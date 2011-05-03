using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Parser
    {
        private Token currentToken;
        private Scanner scanner;

        #region Constructor
        public Parser()
        {

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
        #endregion

        #region Basic Parse Methods
        private BlockName ParseBlockName()
        {
            Identifier i = ParseIdentifier();
            return new BlockName(i);
        }
        private Identifier ParseIdentifier()
        {
            string spelling = currentToken.spelling;
            Accept(Token.TokenType.Identifier);
            return new Identifier(spelling);
        }
        private IntegerLiteral ParseIntegerLiteral()
        {
            string spelling = currentToken.spelling;
            Accept(Token.TokenType.IntegerLiteral);
            return new IntegerLiteral(spelling);
        }
        private BehaviourBlock ParseBehaviourBlock()
        {
            BehaviourBlock bb = null;
            Accept(Token.TokenType.Behaviour);
            if (currentToken.type == Token.TokenType.Assignment)
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
                SingleCommand sc = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
                bb = new BehaviourBlock(bn, sc);
            }
            return bb;
        }
        #endregion

        #region Team File Parse Methods
        private TeamFile ParseTeamFile()
        {
            Accept(Token.TokenType.Team);
            RegimentBlock rb = ParseRegimentBlock();
            return new TeamFile(rb);
        }
        private RegimentBlock ParseRegimentBlock()
        {
            Accept(Token.TokenType.Regiment);
            BlockName bn = ParseBlockName();
            Accept(Token.TokenType.LeftBracket);
            UnitStat us = ParseUnitStat();
            BehaviourBlock bb = ParseBehaviourBlock();
            Accept(Token.TokenType.RightBracket);
            return new RegimentBlock(bn, us, bb);
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
            return sc;
        }
        private SingleCommand ParseControlStructure()
        {
            if (currentToken.type == Token.TokenType.If)
            {
                Expression e = null;
                SingleCommand sc1 = null;
                ControlStructure eif = null;
                SingleCommand sc2 = null;
                AcceptIt();
                Accept(Token.TokenType.LeftParen);
                e = ParseExpression();
                Accept(Token.TokenType.RightParen);
                Accept(Token.TokenType.LeftBracket);
                sc1 = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
                if (currentToken.type == Token.TokenType.ElseIf)
                {
                    AcceptIt();
                    Accept(Token.TokenType.LeftParen);
                    Expression eife = ParseExpression();
                    Accept(Token.TokenType.RightParen);
                    Accept(Token.TokenType.LeftBracket);
                    SingleCommand eifsc = ParseSingleCommand();
                    Accept(Token.TokenType.RightBracket);
                    eif = new ElseIfCommand(eife, eifsc);
                }
                while (currentToken.type == Token.TokenType.ElseIf)
                {
                    AcceptIt();
                    Accept(Token.TokenType.LeftParen);
                    Expression eife = ParseExpression();
                    Accept(Token.TokenType.RightParen);
                    Accept(Token.TokenType.LeftBracket);
                    SingleCommand eifsc = ParseSingleCommand();
                    Accept(Token.TokenType.RightBracket);
                    //eif = new BinaryElseIfCommand(eif, new ElseIfCommand(eife, eifsc));
                }
                if (currentToken.type == Token.TokenType.Else)
                {
                    AcceptIt();
                    Accept(Token.TokenType.LeftBracket);
                    sc2 = ParseSingleCommand();
                    Accept(Token.TokenType.RightBracket);
                }
                return new IfCommand(e, sc1, sc2);
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
                case Token.TokenType.UnitStatName:
                    UnitStat usn = ParseUnitStatName();
                    e = new UnitStatNameExpression(usn);
                    break;
                case Token.TokenType.LeftParen:
                    AcceptIt();
                    e = ParseExpression();
                    Accept(Token.TokenType.RightParen);
                    break;
                case Token.TokenType.RegimentStat:
                    RegimentStat rs = ParseRegimentStat();
                    e = new RegimentStatExpression(rs);
                    break;

            }
            return e;
        }
        private SingleCommand ParseUnitFunction()
        {
            UnitFunctionName ufn = new UnitFunctionName(currentToken.spelling);
            AcceptIt();
            Accept(Token.TokenType.LeftParen);
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.RightParen);
            return new UnitFunction(ufn, i);
        }
        private Operator ParseOperator()
        {
            string spelling = currentToken.spelling;
            Accept(Token.TokenType.Operator);
            return new Operator(spelling);
        }
        private RegimentAssignment ParseRegimentAssignment()
        {
            Accept(Token.TokenType.Regiment);
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.Assignment);
            RegimentSearch rs = ParseRegimentSearch();
            return new RegimentAssignment(i, rs);
        }
        private RegimentStat ParseRegimentStat()
        {
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.Dot);
            UnitStatType ust = ParseUnitStatType();
            return new RegimentStat(i, ust);
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
            string spelling = currentToken.spelling;
            AcceptIt();
            return new UnitStatType(spelling);
        }
        private UnitStat ParseUnitStat()
        {
            UnitStat usn = ParseUnitStatName();
            while (currentToken.type == Token.TokenType.UnitStatName)
            {
                UnitStat usn2 = ParseUnitStatName();
                usn = new BinaryUnitStatName(usn, usn2);
            }
            return usn;
        }
        private UnitStat ParseUnitStatName()
        {
            UnitStat usn = null;
            switch (currentToken.type)
            {
                case Token.TokenType.Size:
                case Token.TokenType.Range:
                case Token.TokenType.Damage:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Health:
                    UnitStatNameVariable sn = new UnitStatNameVariable(currentToken.spelling);
                    AcceptIt();
                    Accept(Token.TokenType.Assignment);
                    IntegerLiteral il = ParseIntegerLiteral();
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatName(sn, il);
                    break;
                case Token.TokenType.RegimentPosition:
                    sn = new UnitStatNameVariable(currentToken.spelling);
                    AcceptIt();
                    Accept(Token.TokenType.Assignment);
                    Accept(Token.TokenType.Position);
                    Accept(Token.TokenType.LeftParen);
                    IntegerLiteral ilx = ParseIntegerLiteral();
                    Accept(Token.TokenType.Comma);
                    IntegerLiteral ily = ParseIntegerLiteral();
                    Accept(Token.TokenType.RightParen);
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatNamePosition(sn, ilx, ily);
                    break;
                case Token.TokenType.Type:
                    sn = new UnitStatNameVariable(currentToken.spelling);
                    AcceptIt();
                    Accept(Token.TokenType.Assignment);
                    AttackType at = ParseAttackType();
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatNameType(sn, at);
                    break;
            }
            return usn;
        }
        private AttackType ParseAttackType()
        {
            if (currentToken.type == Token.TokenType.Melee || currentToken.type == Token.TokenType.Ranged)
            {
                string spelling = currentToken.spelling;
                AcceptIt();
                return new AttackType(spelling);
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
            ParseBlockName();
            Accept(Token.TokenType.LeftBracket);
            ParseGridStat();
            Accept(Token.TokenType.RightBracket);
            return new GridBlock();
        }
        private GridStat ParseGridStat()
        {
            GridStat gsn = ParseGridStatName();
            while (currentToken.type == Token.TokenType.GridStatName)
            {
                GridStat gsn2 = ParseGridStatName();
                gsn = new BinaryGridStatName(gsn, gsn2);
            }
            return gsn;
        }
        private GridStatName ParseGridStatName()
        {
            if (currentToken.type == Token.TokenType.Width || currentToken.type == Token.TokenType.Height)
            {
                GridStatNameVariable gsnv = new GridStatNameVariable(currentToken.spelling);
                AcceptIt();
                Accept(Token.TokenType.Assignment);
                IntegerLiteral il = ParseIntegerLiteral();
                Accept(Token.TokenType.SemiColon);
                return new GridStatName(gsnv, il);
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
            Accept(Token.TokenType.Maximums);
            Accept(Token.TokenType.LeftBracket);
            MaximumsStat ms = ParseMaximumsStat();
            Accept(Token.TokenType.RightBracket);

            return new MaximumsBlock(ms);
        }
        private MaximumsStat ParseMaximumsStat()
        {
            MaximumsStat ms = ParseMaximumsStatName();
            while (currentToken.type == Token.TokenType.MaximumsStatName)
            {
                MaximumsStat ms2 = ParseMaximumsStatName();
                ms = new BinaryMaximumsStatName(ms, ms2);
            }
            return ms;
        }
        private MaximumsStat ParseMaximumsStatName()
        {
            MaximumsStatNameVariable msv = new MaximumsStatNameVariable(currentToken.spelling);
            AcceptIt();
            Accept(Token.TokenType.Assignment);
            IntegerLiteral il = ParseIntegerLiteral();
            Accept(Token.TokenType.SemiColon);
            return new MaximumsStatName(msv, il);
        }
        private StandardsBlock ParseStandardsBlock()
        {
            Accept(Token.TokenType.Standards);
            Accept(Token.TokenType.LeftBracket);
            UnitStat us = ParseUnitStat();
            BehaviourBlock bb = ParseBehaviourBlock();
            Accept(Token.TokenType.RightBracket);
            return new StandardsBlock(us, bb);
        }
        #endregion

    }
}
