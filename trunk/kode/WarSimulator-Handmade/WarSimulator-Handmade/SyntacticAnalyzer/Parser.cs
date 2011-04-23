﻿using System;
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
            ParsePrimaryExpression();
            while (currentToken.type == TokenType.Operator)
            {
                ParseOperator();
                ParsePrimaryExpression();
            }
        }
        private Expression ParsePrimaryExpression()
        {
            switch (currentToken.type)
            {
                case TokenType.IntegerLiteral: ParseIntegerLiteral(); break;
                case TokenType.Operator: ParseOperator(); ParsePrimaryExpression(); break;
                case TokenType.UnitStatName: ParseUnitStatName(); break;
                case TokenType.LeftParen: AcceptIt(); ParseExpression(); Accept(TokenType.RightParen); break;
            }
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
            //Waiting for it to be written in the bnf
        }
        private SingleCommand ParseUnitFunction()
        {
            //Waiting for this to be written in the BNF
        }
        private UnitStat ParseUnitStat()
        {
            while (currentToken.type == TokenType.UnitStatName)
            {
                ParseUnitStatName();
            }
        }
        private UnitStatName ParseUnitStatName()
        {
            switch (currentToken.type)
            {
                case TokenType.Size:
                case TokenType.Range:
                case TokenType.Damage:
                case TokenType.Movement:
                case TokenType.AttackSpeed:
                case TokenType.Health:
                    AcceptIt();
                    Accept(TokenType.Assignment);
                    Accept(TokenType.IntegerLiteral);
                    Accept(TokenType.SemiColon);
                    break;
                case TokenType.RegimentPosition:
                    AcceptIt();
                    Accept(TokenType.Assignment);
                    Accept(TokenType.Position);
                    Accept(TokenType.LeftParen);
                    ParseIntegerLiteral();
                    Accept(TokenType.Comma);
                    ParseIntegerLiteral();
                    Accept(TokenType.RightParen);
                    Accept(TokenType.SemiColon);
                    break;
                case TokenType.Type:
                    AcceptIt();
                    Accept(TokenType.Assignment);
                    ParseAttackType();
                    Accept(TokenType.SemiColon);
                    break;
            }
        }
        private AttackType ParseAttackType()
        {
            if (currentToken.type == TokenType.Melee || currentToken.type == TokenType.Ranged)
            {
                AcceptIt();
            }
        }
        #endregion

        #region Config File Parse Methods
        private void ParseConfigFile()
        {
            Accept(TokenType.Config);
            ParseGridBlock();
            ParseRulesBlock();
        }
        private void ParseGridBlock()
        {
            Accept(TokenType.Grid);
            ParseBlockName();
            Accept(TokenType.LeftBracket);
            ParseGridStat();
            Accept(TokenType.RightBracket);
        }
        private void ParseGridStat()
        {
            while (currentToken.type == TokenType.GridStatName)
            {
                ParseGridStatName();
            }
        }
        private void ParseGridStatName()
        {
            if (currentToken.type == TokenType.Width || currentToken.type == TokenType.Height)
            {
                AcceptIt();
                Accept(TokenType.Assignment);
                ParseIntegerLiteral();
                Accept(TokenType.SemiColon);
            }
        }

        private void ParseRulesBlock()
        {
            ParseStandardsBlock();
            ParseMaximumsBlock();
        }
        private void ParseMaximumsBlock()
        {
            Accept(TokenType.Regiments);
            Accept(TokenType.LeftBracket);
            ParseMaximumsStat();
            Accept(TokenType.RightBracket);
        }
        private void ParseMaximumsStat()
        {
            while (currentToken.type == TokenType.MaximumsStatName || currentToken.type == TokenType.UnitStatName)
            {
                if (currentToken.type == TokenType.UnitStatName) { ParseUnitStatName(); }
                else if (currentToken.type == TokenType.MaximumsStatName) { ParseMaximumsStatName(); }
            }
        }
        private void ParseMaximumsStatName()
        {
            if (currentToken.type == TokenType.Regiments ||
                currentToken.type == TokenType.Teams)
            {
                AcceptIt();
                Accept(TokenType.Assignment);
                ParseIntegerLiteral();
                Accept(TokenType.SemiColon);
            }
        }
        private void ParseStandardsBlock()
        {
            Accept(TokenType.Standards);
            Accept(TokenType.LeftBracket);
            ParseUnitStat();
            ParseBehaviourBlock();
            Accept(TokenType.RightBracket);
        }
        #endregion

    }
}