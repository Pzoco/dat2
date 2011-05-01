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
            UnitFunction,
            UnitStatName,
            GridStatName,
            MaximumsStatName,

            //-----ALPHEBETICAL BEGIN-----
            AttackSpeed,
            Behaviour,
            Config,
            Damage,
            Else,
            Grid,
            Health,
            Height,
            If,
            Maximums,
            Melee,
            Movement,
            Position,
            Range,
            Ranged,
            Regiment,
            RegimentPosition,
            Regiments,
            SearchForEnemies,
            SearchForFriends,
            Size,
            Standards,
            Team,
            Teams,
            Type,
            While,
            Width,
            //-----ALPHEBETICAL END-----

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
            "<integer-literal>",
            "<operator>",
            "<unitfunction>", //Attack, MoveTowards, MoveAway = Tokens, eller?
            "<unitstatname>",
            "<maximumsstatname>",
            "<gridstatname>",
                //   ALPHABETICAL START
            "AttackSpeed",
            "Behaviour",
            "Config",
            "Damage",
            "Else",
            "Grid",
            "Health",
            "Height",
            "If",
            "Maximums",
            "Melee",
            "Movement", 
            "Position",
            "Range",
            "Ranged",
            "Regiment",
            "Regiments",
            "RegimentPosition",
            "SearchForEnemies",
            "SearchForFriends",
            "Size", 
            "Standards",
            "Team",
            "Teams",
            "Type",
            "While",
            "Width",
                //   ALPHABETICAL END  
            //Signs
            "/",
            "(",
            ")",
            "{",
            "}",
            ",",
            ";",
            "="
        };
    }
}
