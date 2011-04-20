using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public enum TokenType
    {
        Identifier,
        IntegerLiteral,
        Operator,

        //Units
        UnitFunction,
        UnitStatName,

        //Control Structures
        If,
        Else,
        While,

        //Block Types
        Team,
        Regiment,
        Behaviour,

        //Signs
        Slash,
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        Assignment,
    }
    class Token
    {
        public TokenType type;
        public string spelling;
    }
}
