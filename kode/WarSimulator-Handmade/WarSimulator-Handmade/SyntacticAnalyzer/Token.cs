using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    
    class Token
    {
        public TokenType type;
        public string spelling;
        public SourcePosition position;

        public Token(TokenType type, string spelling, SourcePosition position)
        {
            if (type == TokenType.Identifier)
            {

            }
            this.type = type;
            this.spelling = spelling;
        }

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
            SearchForEnemies,

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
        private static string[] TokenTable = new string[]
        {
            "<identifier>",
            "<operator>",
            "<integer-literal>",
            "Team",
            "Regiment",
            "Behaviour",
            "SearchForEnemies",
            "SearchForFriends",
            "Attack",
            "MoveTowards",
            "MoveAway",
            "Size",
            "Type",
            "AttackSpeed",
            "Health",
            "Melee",
            "Ranged",
            "Range",
            "Damage",
            "Movement",
            "RegimentPosition",
            "Position",
            "Config",
            "Grid",
            "Width",
            "Height",

            "if",
            "else",
            "while",
            "{",
            "}",
            "(",
            ")",
            ";",


        }
    }
}
