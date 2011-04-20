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
        private void Accept(TokenType type)
        {
            if (currentToken.type == type)
            {
                currentToken = scanner.Scan();
            }
            else
            {
                Console.WriteLine("Error - Expected {0}, but got {1}", type, currentToken.type);
            }
        }
        private void AcceptIt()
        {
            currentToken = scanner.Scan();
        }
        #endregion

        #region Basic Parse Methods
        private void ParseBlockName()
        {
            ParseIdentifier();
        }
        private void ParseIdentifier()
        {
            Accept(TokenType.Identifier);
        }
        private void ParseIntegerLiteral()
        {
            Accept(TokenType.IntegerLiteral);
        }
        private void ParseBehaviourBlock()
        {
            Accept(TokenType.Behaviour);
            if (currentToken.type == TokenType.Assignment)
            {
                AcceptIt();
                ParseBlockName();
            }
            else
            {
                ParseBlockName();
                Accept(TokenType.LeftBracket);
                ParseSingleCommand();
                Accept(TokenType.RightBracket);
            }
        }
        #endregion

        #region Team File Parse Methods
        private void ParseTeamFile()
        {
            Accept(TokenType.Team);
            ParseRegimentBlock();
        }
        private void ParseRegimentBlock()
        {
            Accept(TokenType.Regiment);
            ParseBlockName();
            Accept(TokenType.LeftBracket);
            ParseUnitStat();
            ParseBehaviourBlock();
            Accept(TokenType.RightBracket);
        }
        private void ParseSingleCommand()
        {
            if (currentToken.type == TokenType.If || currentToken.type == TokenType.While)
            {
                ParseControlStructure();
            }
            else if (currentToken.type == TokenType.UnitFunction)
            {
                ParseUnitFunction();
            }
        }
        private void ParseControlStructure()
        {
            if (currentToken.type == TokenType.If)
            {
                AcceptIt();
                Accept(TokenType.LeftParen);
                ParseExpression();
                Accept(TokenType.RightParen);
                Accept(TokenType.LeftBracket);
                ParseSingleCommand();
                Accept(TokenType.RightBracket);
                if (currentToken.type == TokenType.Else)
                {
                    Accept(TokenType.LeftBracket);
                    ParseSingleCommand();
                    Accept(TokenType.RightBracket);
                }
            }
            else if (currentToken.type == TokenType.While)
            {
                AcceptIt();
                Accept(TokenType.LeftParen);
                ParseExpression();
                Accept(TokenType.RightParen);
                Accept(TokenType.LeftBracket);
                ParseSingleCommand();
                Accept(TokenType.RightBracket);
            }
        }
        private void ParseExpression()
        {
            ParsePrimaryExpression();
            while (currentToken.type == TokenType.Operator)
            {
                ParseOperator();
                ParsePrimaryExpression();
            }
        }
        private void ParsePrimaryExpression()
        {
            switch (currentToken.type)
            {
                case TokenType.IntegerLiteral: ParseIntegerLiteral(); break;
                case TokenType.Operator: ParseOperator(); ParsePrimaryExpression(); break;
                case TokenType.UnitStatName: ParseUnitStatName(); break;
                case TokenType.LeftParen: AcceptIt(); ParseExpression(); Accept(TokenType.RightParen); break;
            }
        }
        private void ParseOperator()
        {
            
        }
        private void ParseUnitFunction();
        private void ParseUnitStat();
        private void ParseUnitStatName();
        private void ParseAttackType();
        #endregion

        #region Config File Parse Methods
        private void ParseConfigFile();
        private void ParseGridBlock();
        private void ParseGridStat();
        private void ParseGridStatName();
        private void ParseRulesBlock();
        private void ParseMaximumsBlock();
        private void ParseMaximumsStat();
        private void ParseMaximumsStatName();
        private void ParseStandardsBlock();
        #endregion

    }
}
