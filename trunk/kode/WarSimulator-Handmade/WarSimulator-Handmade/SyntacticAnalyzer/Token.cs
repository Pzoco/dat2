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
        Position,
        Regiments,

        //Units
        UnitFunction,
        UnitStatName,

        //Grid
        GridStatName,
        Width,
        Height,

        //Maximums
        Maximums,
        MaximumsStatName,
        Teams,

        //UnitStatName
        Size, Range, Damage,
        Movement, AttackSpeed, Health,
        RegimentPosition,
        Type,

        //Attack Type
        Melee,
        Ranged,

        //Control Structures
        If,
        Else,
        While,

        //Block Types
        Team,
        Regiment,
        Behaviour,
        Config,
        Grid,
        Standards,

        //Signs
        Slash,
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        Comma,
        SemiColon,
        Assignment,
    }
    class Token
    {
        public TokenType type;
        public string spelling;

        public Token(TokenType type, string spelling)
        {
            this.type = type;
            this.spelling = spelling;
        }
    }
}
