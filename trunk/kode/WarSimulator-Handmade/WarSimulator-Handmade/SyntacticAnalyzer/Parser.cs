﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Parser
    {
        private Scanner scanner;
        private ErrorReporter errorReporter;
        private Token currentToken;
        private SourcePosition previousTokenPosition;

        #region Constructor
        public Parser(Scanner lexer, ErrorReporter reporter)
        {
            scanner = lexer;
            errorReporter = reporter;
            previousTokenPosition = new SourcePosition();
        }
        #endregion

        #region Accept Methods
        //Accepts a token if it matches the expected type
        private void Accept(Token.TokenType expectedType)
        {
            if (currentToken.type == expectedType)
            {
                currentToken = scanner.Scan();
            }
            else
            {
                Console.WriteLine("Error - Expected {0}, but got {1}", expectedType, currentToken.type);
            }
        }

        //Accepts a token
        private void AcceptIt()
        {
            currentToken = scanner.Scan();
        }

        void Start(SourcePosition position)
        {
            position.start = currentToken.position.start;
        }

        void Finish(SourcePosition position)
        {
            position.finish = previousTokenPosition.finish;
        }
        #endregion

        #region Basic Parse Methods
        private BlockName ParseBlockName()
        {
            Identifier i = ParseIdentifier();
            return new BlockName(i);
        }
        private Identifier ParseIdentifier()
        {
            Identifier id = null;

            if (currentToken.type == Token.TokenType.Identifier)
            {
                previousTokenPosition = currentToken.position;
                String spelling = currentToken.spelling;
                id = new Identifier(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            }
            else
            {
                Console.WriteLine("Error parsing Identifier");
            }
            return id;
        }
        private IntegerLiteral ParseIntegerLiteral()
        {
            previousTokenPosition = currentToken.position;
            string spelling = currentToken.spelling;
            Accept(Token.TokenType.IntegerLiteral);
            return new IntegerLiteral(spelling,previousTokenPosition);
        }
        private BehaviourBlock ParseBehaviourBlock()
        {
            BehaviourBlock bb = null;
            Accept(Token.TokenType.Behaviour);
            if (currentToken.type == Token.TokenType.Operator)
            {
                AcceptIt();
                BlockName bn = ParseBlockName();
                Accept(Token.TokenType.SemiColon);
                bb = new BehaviourAssignment(bn);
            }
            else
            {
                BlockName bn = ParseBlockName();
                Accept(Token.TokenType.LeftBracket);
                List<SingleCommand> scs = new List<SingleCommand>();
                while (currentToken.type == Token.TokenType.If || currentToken.type == Token.TokenType.While ||
                        currentToken.type == Token.TokenType.UnitFunction || currentToken.type == Token.TokenType.Regiment)
                {
                    scs.Add(ParseSingleCommand());
                }
                Accept(Token.TokenType.RightBracket);
                bb = new BehaviourBlock(bn, scs);
            }
            return bb;
        }
        #endregion

        #region Team File Parse Methods
        public TeamFile ParseTeamFile()
        {
            TeamFile tf = null;
            currentToken = scanner.Scan();

            Accept(Token.TokenType.Team);
            Identifier id = ParseIdentifier();
            try
            {
                SourcePosition regimentBlockPosition = new SourcePosition();
                Start(regimentBlockPosition);
                RegimentBlock rb = ParseRegimentBlock();
                while (currentToken.type==Token.TokenType.RightBracket)
                {
                    AcceptIt();
                    RegimentBlock rb2 = ParseRegimentBlock();
                    Finish(regimentBlockPosition);
                    rb = new SequentialRegimentBlock(rb, rb2, regimentBlockPosition);
                }
                tf = new TeamFile(rb, previousTokenPosition);
                if (currentToken.type != Token.TokenType.EndOfText)
                {
                    Console.WriteLine("Error! File did not end when expected! " + currentToken.spelling);
                }
            }
            catch (Exception ex) { return null; }
            return tf;
        }

        private RegimentBlock ParseRegimentBlock()
        {
            Accept(Token.TokenType.Regiment);
            BlockName bn = ParseBlockName();
            Accept(Token.TokenType.LeftBracket);
            List<UnitStatDeclaration> usds = new List<UnitStatDeclaration>();
            bool declarationFound = true;
            while (declarationFound)
            {
                declarationFound = false;
                switch (currentToken.type)
                {
                    case Token.TokenType.Size:
                    case Token.TokenType.Range:
                    case Token.TokenType.Damage:
                    case Token.TokenType.Movement:
                    case Token.TokenType.AttackSpeed:
                    case Token.TokenType.Health:
                    case Token.TokenType.RegimentPosition:
                    case Token.TokenType.Type:
                        usds.Add(ParseUnitStatDeclaration()); declarationFound = true; break;
                }
            }
            BehaviourBlock bb = ParseBehaviourBlock();
            Accept(Token.TokenType.RightBracket);
            return new RegimentBlock(bn, usds, bb);
        }
        private SingleCommand ParseSingleCommand()
        {
            SingleCommand sc = null;
            if (currentToken.type == Token.TokenType.If || currentToken.type == Token.TokenType.While)
            {
                sc = ParseControlStructure();
            }
            else if (currentToken.type == Token.TokenType.UnitFunction)
            {
                sc = ParseUnitFunction();
            }
            else if (currentToken.type == Token.TokenType.Regiment)
            {
                sc = ParseRegimentAssignment();
            }
            else if (currentToken.type == Token.TokenType.Attack || currentToken.type == Token.TokenType.MoveAway || currentToken.type== Token.TokenType.MoveTowards)
            {
                sc = ParseUnitFunction();
            }
            return sc;
        }
        private SingleCommand ParseControlStructure()
        {
            if (currentToken.type == Token.TokenType.If)
            {
                Expression e = null;
                SingleCommand sc1 = null;
                SingleCommand sc2 = null;
                List<ElseIfCommand> eifc = new List<ElseIfCommand>();
                AcceptIt();
                Accept(Token.TokenType.LeftParen);
                e = ParseExpression();
                Accept(Token.TokenType.RightParen);
                Accept(Token.TokenType.LeftBracket);
                sc1 = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
                while (currentToken.type == Token.TokenType.Else)
                {
                    AcceptIt();
                    if (currentToken.type == Token.TokenType.If)
                    {
                        AcceptIt();
                        Accept(Token.TokenType.LeftParen);
                        Expression eife = ParseExpression();
                        Accept(Token.TokenType.RightParen);
                        Accept(Token.TokenType.LeftBracket);
                        SingleCommand eifsc = ParseSingleCommand();
                        Accept(Token.TokenType.RightBracket);
                        eifc.Add(new ElseIfCommand(eife, eifsc));
                    }
                    else
                    {
                        Accept(Token.TokenType.LeftBracket);
                        sc2 = ParseSingleCommand();
                        Accept(Token.TokenType.RightBracket);
                        break;
                    }
                }
                return new IfCommand(e, sc1, sc2, eifc);
            }
            else if (currentToken.type == Token.TokenType.While)
            {
                Expression e = null;
                SingleCommand sc = null;
                AcceptIt();
                Accept(Token.TokenType.LeftParen);
                e = ParseExpression();
                Accept(Token.TokenType.RightParen);
                Accept(Token.TokenType.LeftBracket);
                sc = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
                return new WhileCommand(e, sc);
            }
            return null;
        }
        private Expression ParseExpression()
        {
            Expression e1 = ParsePrimaryExpression();
            while (currentToken.type == Token.TokenType.Operator)
            {
                Operator o = ParseOperator();
                Expression e2 = ParsePrimaryExpression();
                e1 = new BinaryExpression(e1, e2, o);
            }
            return e1;
        }
        private Expression ParsePrimaryExpression()
        {
            Expression e = null;
            switch (currentToken.type)
            {
                case Token.TokenType.IntegerLiteral:
                    IntegerLiteral il = ParseIntegerLiteral();
                    e = new IntegerExpression(il);
                    break;
                case Token.TokenType.Operator:
                    Operator o = ParseOperator();
                    Expression pe = ParsePrimaryExpression();
                    e = new UnaryExpression(o, pe);
                    break;
                case Token.TokenType.Size:
                case Token.TokenType.Range:
                case Token.TokenType.Damage:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Health:
                case Token.TokenType.RegimentPosition:
                case Token.TokenType.Type:
                    UnitStatType usd = ParseUnitStatType();
                    e = new UnitStatVNameExpression(usd);
                    break;
                case Token.TokenType.LeftParen:
                    AcceptIt();
                    e = ParseExpression();
                    Accept(Token.TokenType.RightParen);
                    break;
                case Token.TokenType.Identifier:
                    e = ParseRegimentStat();
                    break;
            }
            return e;
        }
        private SingleCommand ParseUnitFunction()
        {
            previousTokenPosition = currentToken.position;
            UnitFunctionName ufn = new UnitFunctionName(currentToken.spelling,previousTokenPosition);
            AcceptIt();
            Accept(Token.TokenType.LeftParen);
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.RightParen);
            Accept(Token.TokenType.SemiColon);
            return new UnitFunction(ufn, i);
        }
        private Operator ParseOperator()
        {
            previousTokenPosition = currentToken.position;
            string spelling = currentToken.spelling;
            Accept(Token.TokenType.Operator);
            return new Operator(spelling,previousTokenPosition);
        }
        private RegimentDeclaration ParseRegimentAssignment()
        {
            Accept(Token.TokenType.Regiment);
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.Operator);
            RegimentSearch rs = ParseRegimentSearch();
            Accept(Token.TokenType.SemiColon);
            return new RegimentDeclaration(i, rs);
        }
        private RegimentStatExpression ParseRegimentStat()
        {
            RegimentStat e = null;
            UnitStatType ust = null;
            Identifier id = ParseIdentifier();
            Accept(Token.TokenType.Dot);
            switch (currentToken.type)
            {
                case Token.TokenType.Range:
                case Token.TokenType.Health:
                case Token.TokenType.Size:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Distance:
                case Token.TokenType.Damage:
                    ust = ParseUnitStatType();
                    e = new RegimentStat(id,ust); break;
            }
            return new RegimentStatExpression(e); ;
        }
        private RegimentSearch ParseRegimentSearch()
        {
            Accept(Token.TokenType.SearchForEnemies);
            Accept(Token.TokenType.LeftParen);
            Parameters p = ParseParameters();
            Accept(Token.TokenType.RightParen);

            return new RegimentSearch(p);
        }
        private Parameters ParseParameters()
        {
            UnitStatType ust = ParseUnitStatType();
            Operator o = ParseOperator();
            IntegerLiteral il = ParseIntegerLiteral();
            Parameters p = new Parameter(ust, o, il);

            while (currentToken.type == Token.TokenType.Comma)
            {
                UnitStatType ust2 = ParseUnitStatType();
                Operator o2 = ParseOperator();
                IntegerLiteral il2 = ParseIntegerLiteral();
                Parameters p2 = new Parameter(ust2, o2, il2);

                p = new BinaryParameter(p, p2);
            }
            return p;
        }
        private UnitStatType ParseUnitStatType()
        {
            previousTokenPosition = currentToken.position;
            string spelling = currentToken.spelling;
            AcceptIt();
            return new UnitStatType(spelling,previousTokenPosition);
        }
        private UnitStatDeclaration ParseUnitStatDeclaration()
        {
            UnitStatDeclaration usn = null;
            previousTokenPosition = currentToken.position;
            switch (currentToken.type)
            {
                case Token.TokenType.Size:
                case Token.TokenType.Range:
                case Token.TokenType.Damage:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Health:
                    UnitStatVName sn = new UnitStatVName(currentToken.spelling,previousTokenPosition);
                    AcceptIt();
                    Accept(Token.TokenType.Operator);
                    IntegerLiteral il = ParseIntegerLiteral();
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatIntegerDeclaration(sn, il);
                    break;
                case Token.TokenType.RegimentPosition:
                    sn = new UnitStatVName(currentToken.spelling,previousTokenPosition);
                    AcceptIt();
                    Accept(Token.TokenType.Operator);
                    Accept(Token.TokenType.Position);
                    Accept(Token.TokenType.LeftParen);
                    IntegerLiteral ilx = ParseIntegerLiteral();
                    Accept(Token.TokenType.Comma);
                    IntegerLiteral ily = ParseIntegerLiteral();
                    Accept(Token.TokenType.RightParen);
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatPositionDeclaration(sn, ilx, ily);
                    break;
                case Token.TokenType.Type:
                    sn = new UnitStatVName(currentToken.spelling,previousTokenPosition);
                    AcceptIt();
                    Accept(Token.TokenType.Operator);
                    AttackType at = ParseAttackType();
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatTypeDeclaration(sn, at);
                    break;
            }
            return usn;
        }
        private AttackType ParseAttackType()
        {
            if (currentToken.type == Token.TokenType.Melee || currentToken.type == Token.TokenType.Ranged)
            {
                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                AcceptIt();
                return new AttackType(spelling,previousTokenPosition);
            }
            return null;
        }
        #endregion

        #region Config File Parse Methods
        private ConfigFile ParseConfigFile()
        {
            Accept(Token.TokenType.Config);
            GridBlock gb = ParseGridBlock();
            RulesBlock rb = ParseRulesBlock();
            return new ConfigFile(gb, rb);
        }
        private GridBlock ParseGridBlock()
        {
            Accept(Token.TokenType.Grid);
            BlockName bn = ParseBlockName();
            Accept(Token.TokenType.LeftBracket);
            List<GridStatDeclaration> gsd = new List<GridStatDeclaration>();
            while (currentToken.type == Token.TokenType.Width || currentToken.type == Token.TokenType.Height)
            {
                gsd.Add(ParseGridStatDeclaration());
            }
            Accept(Token.TokenType.RightBracket);
            return new GridBlock(bn, gsd);
        }
        private GridStatDeclaration ParseGridStatDeclaration()
        {
            if (currentToken.type == Token.TokenType.Width || currentToken.type == Token.TokenType.Height)
            {
                previousTokenPosition = currentToken.position;
                GridStatVName gsnv = new GridStatVName(currentToken.spelling,previousTokenPosition);
                AcceptIt();
                Accept(Token.TokenType.Operator);
                IntegerLiteral il = ParseIntegerLiteral();
                Accept(Token.TokenType.SemiColon);
                return new GridStatDeclaration(gsnv, il);
            }
            return null;
        }

        private RulesBlock ParseRulesBlock()
        {
            MaximumsBlock mb = ParseMaximumsBlock();
            StandardsBlock sb = ParseStandardsBlock();
            return new RulesBlock(mb, sb);
        }
        private MaximumsBlock ParseMaximumsBlock()
        {
            Accept(Token.TokenType.Maxima);
            Accept(Token.TokenType.LeftBracket);
            List<MaximumsStatDeclaration> msds = new List<MaximumsStatDeclaration>();
            bool declarationFound = true;
            while (declarationFound)
            {
                declarationFound = false;
                switch (currentToken.type)
                {
                    case Token.TokenType.Size:
                    case Token.TokenType.Range:
                    case Token.TokenType.Damage:
                    case Token.TokenType.Movement:
                    case Token.TokenType.AttackSpeed:
                    case Token.TokenType.Health:
                    case Token.TokenType.Regiments:
                    case Token.TokenType.Teams:
                        msds.Add(ParseMaximumsStatDeclaration());
                        declarationFound = true; break;
                }
            }
            Accept(Token.TokenType.RightBracket);

            return new MaximumsBlock(msds);
        }
        private MaximumsStatDeclaration ParseMaximumsStatDeclaration()
        {
            if (currentToken.type == Token.TokenType.Regiments || currentToken.type == Token.TokenType.Teams)
            {
                previousTokenPosition = currentToken.position;
                MaximumsStatVName msv = new MaximumsStatVName(currentToken.spelling,previousTokenPosition);
                AcceptIt();
                Accept(Token.TokenType.Operator);
                IntegerLiteral il = ParseIntegerLiteral();
                Accept(Token.TokenType.SemiColon);
                return new MaximumsStatDeclaration(msv, il);
            }
            return null;
        }
        private StandardsBlock ParseStandardsBlock()
        {
            Accept(Token.TokenType.Standards);
            Accept(Token.TokenType.LeftBracket);
            List<UnitStatDeclaration> usds = new List<UnitStatDeclaration>();
            bool declarationFound = true;
            while (declarationFound)
            {
                declarationFound = false;
                switch (currentToken.type)
                {
                    case Token.TokenType.Size:
                    case Token.TokenType.Range:
                    case Token.TokenType.Damage:
                    case Token.TokenType.Movement:
                    case Token.TokenType.AttackSpeed:
                    case Token.TokenType.Health:
                    case Token.TokenType.RegimentPosition:
                    case Token.TokenType.Type:
                        usds.Add(ParseUnitStatDeclaration()); declarationFound = true; break;
                }
            }
            BehaviourBlock bb = ParseBehaviourBlock();
            Accept(Token.TokenType.RightBracket);
            return new StandardsBlock(usds, bb);
        }
        #endregion

    }
}
