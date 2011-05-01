using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    
    class Token
    {
        public TokenType Type;
        public string Spelling;
        public SourcePosition Position;

        public Token(TokenType Type, string Spelling, SourcePosition Position)
        {
            if (Type == TokenType.Identifier)
            {
                TokenType CurrentType = FirstReservedWord;
                bool Searching = true;

                while (Searching)
                {
                    int Comparison = TokenTable[(int)CurrentType].CompareTo(Spelling);
                    if (Comparison == 0)
                    {
                        this.Type = CurrentType;
                        Searching = false;
                    }
                    else if (Comparison > 0 || CurrentType == LastReservedWord)
                    {
                        this.Type = Token.TokenType.Identifier;
                        Searching = false;
                    }
                    else
                    {
                        CurrentType++;
                        //USIKKERT OM DETTE ER KORREKT NÅR VI ITERERERERERER IGENNEM EN ENUM
                    }
                }

            }
            this.Type = Type;
            this.Spelling = Spelling;
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
        //OBS: HVIS ATTACK, MOVETOWARDS, MOVEAWAY SKAL MED I TOKENTABLE SKAL FIRSTRESERVED ÆNDRES TIL ATTACK
        private readonly static TokenType FirstReservedWord = Token.TokenType.AttackSpeed,
                      LastReservedWord = Token.TokenType.Width;
        // OBS: DET ER HURTIGERE AT CASTE TIL INTS HER END INDE I WHILE LOOPS, JFV. MINI-TRIANGLE
    }
}
