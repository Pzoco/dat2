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
        private void Accept(TokenType expectedType)
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
            Accept(TokenType.Identifier);
            return new Identifier(spelling);
        }
        private IntegerLiteral ParseIntegerLiteral()
        {
            string spelling = currentToken.spelling;
            Accept(TokenType.IntegerLiteral);
            return new IntegerLiteral(spelling);
        }
        private BehaviourBlock ParseBehaviourBlock()
        {
            BehaviourBlock bb = null;
            Accept(TokenType.Behaviour);
            if (currentToken.type == TokenType.Assignment)
            {
                AcceptIt();
                BlockName bn = ParseBlockName();
                Accept(TokenType.SemiColon);
                bb = new BehaviourAssignment(bn);
            }
            else
            {
                BlockName bn = ParseBlockName();
                Accept(TokenType.LeftBracket);
                SingleCommand sc = ParseSingleCommand();
                Accept(TokenType.RightBracket);
                bb = new BehaviourBlock(bn, sc);
            }
            return bb;
        }
        #endregion

        #region Team File Parse Methods
        private TeamFile ParseTeamFile()
        {
            Accept(TokenType.Team);
            RegimentBlock rb = ParseRegimentBlock();
            return new TeamFile(rb);
        }
        private RegimentBlock ParseRegimentBlock()
        {
            Accept(TokenType.Regiment);
            BlockName bn = ParseBlockName();
            Accept(TokenType.LeftBracket);
            UnitStat us = ParseUnitStat();
            BehaviourBlock bb = ParseBehaviourBlock();
            Accept(TokenType.RightBracket);
            return new RegimentBlock(bn, us, bb);
        }
        private SingleCommand ParseSingleCommand()
        {
            SingleCommand sc = null;
            if (currentToken.type == TokenType.If || currentToken.type == TokenType.While)
            {
                sc = ParseControlStructure();
            }
            else if (currentToken.type == TokenType.UnitFunction)
            {
                sc = ParseUnitFunction();
            }
            return sc;
        }
        private SingleCommand ParseControlStructure()
        {
            if (currentToken.type == TokenType.If)
            {
                Expression e = null;
                SingleCommand sc1 = null;
                SingleCommand sc2 = null;
                AcceptIt();
                Accept(TokenType.LeftParen);
                e = ParseExpression();
                Accept(TokenType.RightParen);
                Accept(TokenType.LeftBracket);
                sc1 = ParseSingleCommand();
                Accept(TokenType.RightBracket);
                if (currentToken.type == TokenType.Else)
                {
                    Accept(TokenType.LeftBracket);
                    ParseSingleCommand();
                    Accept(TokenType.RightBracket);
                }
                return new IfCommand(e, sc1, sc2);
            }
            else if (currentToken.type == TokenType.While)
            {
                Expression e = null;
                SingleCommand sc = null;
                AcceptIt();
                Accept(TokenType.LeftParen);
                ParseExpression();
                Accept(TokenType.RightParen);
                Accept(TokenType.LeftBracket);
                ParseSingleCommand();
                Accept(TokenType.RightBracket);
                return new WhileCommand(e, sc);
            }
            return null;
        }
        private Expression ParseExpression()
        {
            Expression e1 = ParsePrimaryExpression();
            while (currentToken.type == TokenType.Operator)
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
                case TokenType.IntegerLiteral: 
                    IntegerLiteral il = ParseIntegerLiteral();
                    e = new IntegerExpression(il);
                    break;
                case TokenType.Operator: 
                    Operator o = ParseOperator(); 
                    Expression pe = ParsePrimaryExpression();
                    e = new UnaryExpression(o, pe);
                    break;
                case TokenType.UnitStatName: 
                    UnitStat usn = ParseUnitStatName();
                    e = new UnitStatNameExpression(usn);
                    break;
                case TokenType.LeftParen: 
                    AcceptIt(); 
                    e = ParseExpression(); 
                    Accept(TokenType.RightParen); 
                    break;
            }
            return e;
        }
        private SingleCommand ParseUnitFunction()
        {
            //Waiting for this to be written
            return null;
        }
        private Operator ParseOperator()
        {
            string spelling = currentToken.spelling;
            Accept(TokenType.Operator);
            return new Operator(spelling);
        }
        private RegimentAssignment ParseRegimentAssignment()
        {
            Accept(TokenType.Regiment);
            Accept(TokenType.Identifier);
            Accept(TokenType.Assignment);
            RegimentSearch rs = ParseRegimentSearch();
            return new RegimentAssignment(rs);
        }
        private RegimentSearch ParseRegimentSearch()
        {
            string spelling = currentToken.spelling;
            AcceptIt();
            return new RegimentSearch(spelling);
        }
        private UnitStat ParseUnitStat()
        {
            UnitStat usn = ParseUnitStatName();
            while (currentToken.type == TokenType.UnitStatName)
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
                case TokenType.Size:
                case TokenType.Range:
                case TokenType.Damage:
                case TokenType.Movement:
                case TokenType.AttackSpeed:
                case TokenType.Health:
                    UnitStatNameVariable sn = new UnitStatNameVariable(currentToken.spelling);
                    AcceptIt();
                    Accept(TokenType.Assignment);
                    IntegerLiteral il = ParseIntegerLiteral();
                    Accept(TokenType.SemiColon);
                    usn = new UnitStatName(sn, il);
                    break;
                case TokenType.RegimentPosition:
                    sn = new UnitStatNameVariable(currentToken.spelling);
                    AcceptIt();
                    Accept(TokenType.Assignment);
                    Accept(TokenType.Position);
                    Accept(TokenType.LeftParen);
                    IntegerLiteral ilx = ParseIntegerLiteral();
                    Accept(TokenType.Comma);
                    IntegerLiteral ily = ParseIntegerLiteral();
                    Accept(TokenType.RightParen);
                    Accept(TokenType.SemiColon);
                    usn = new UnitStatNamePosition(sn, ilx, ily);
                    break;
                case TokenType.Type:
                    sn = new UnitStatNameVariable(currentToken.spelling);
                    AcceptIt();
                    Accept(TokenType.Assignment);
                    AttackType at = ParseAttackType();
                    Accept(TokenType.SemiColon);
                    usn = new UnitStatNameType(sn, at);
                    break;
            }
            return usn;
        }
        private AttackType ParseAttackType()
        {
            if (currentToken.type == TokenType.Melee || currentToken.type == TokenType.Ranged)
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
            Accept(TokenType.Config);
            GridBlock gb = ParseGridBlock();
            RulesBlock rb = ParseRulesBlock();
            return new ConfigFile(gb, rb);
        }
        private GridBlock ParseGridBlock()
        {
            Accept(TokenType.Grid);
            ParseBlockName();
            Accept(TokenType.LeftBracket);
            ParseGridStat();
            Accept(TokenType.RightBracket);
            return new GridBlock();
        }
        private GridStat ParseGridStat()
        {
            GridStat gsn = ParseGridStatName();
            while (currentToken.type == TokenType.GridStatName)
            {
                GridStat gsn2 = ParseGridStatName();
                gsn = new BinaryGridStatName(gsn,gsn2);
            }
            return gsn;
        }
        private GridStatName ParseGridStatName()
        {
            if (currentToken.type == TokenType.Width || currentToken.type == TokenType.Height)
            {
                GridStatNameVariable gsnv = new GridStatNameVariable(currentToken.spelling);
                AcceptIt();
                Accept(TokenType.Assignment);
                IntegerLiteral il = ParseIntegerLiteral();
                Accept(TokenType.SemiColon);
                return new GridStatName(gsnv, il);
            }
            return null;
        }

        private RulesBlock ParseRulesBlock()
        {
            MaximumsBlock mb = ParseMaximumsBlock();
            StandardsBlock sb = ParseStandardsBlock();
            return new RulesBlock(mb,sb);
        }
        private MaximumsBlock ParseMaximumsBlock()
        {
            Accept(TokenType.Maximums);
            Accept(TokenType.LeftBracket);
            ParseMaximumsStat();
            Accept(TokenType.RightBracket);

            return new MaximumsBlock();
        }
        private MaximumsStat ParseMaximumsStat()
        {
            MaximumsStatName ms = null;
            UnitStat us = null;
            while (currentToken.type == TokenType.MaximumsStatName || currentToken.type == TokenType.UnitStatName)
            {
                if (currentToken.type == TokenType.UnitStatName)
                {
                    if (us == null)
                    {
                        us = ParseUnitStatName();
                    }
                    else
                    {
                        UnitStat us2 = ParseUnitStatName();
                        us = new BinaryUnitStatName(us, us2);
                    }
                }
                else if (currentToken.type == TokenType.MaximumsStatName) 
                {
                    if (ms == null)
                    {
                        ms = ParseMaximumsStatName();
                    }
                    else
                    {
                        MaximumsStatName ms2 = ParseMaximumsStatName();
                        ms = new BinaryMaximumsStatName(ms, ms2);
                    }
                }
            }
            return new MaximumsStat(us,ms);
        }
        private MaximumsStatName ParseMaximumsStatName()
        {
            if (currentToken.type == TokenType.Regiments ||
                currentToken.type == TokenType.Teams)
            {
                MaximumsStatNameVariable msv = new MaximumsStatNameVariable(currentToken.spelling);
                AcceptIt();
                Accept(TokenType.Assignment);
                IntegerLiteral il = ParseIntegerLiteral();
                Accept(TokenType.SemiColon);
                return new MaximumsStatName(msv, il);
            }
            return null;
        }
        private StandardsBlock ParseStandardsBlock()
        {
            Accept(TokenType.Standards);
            Accept(TokenType.LeftBracket);
            UnitStat us = ParseUnitStat();
            BehaviourBlock bb = ParseBehaviourBlock();
            Accept(TokenType.RightBracket);
            return new StandardsBlock(us, bb);
        }
        #endregion

    }
}
