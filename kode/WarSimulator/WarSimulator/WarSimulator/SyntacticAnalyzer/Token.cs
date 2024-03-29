﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    
    public class Token
    {
        public TokenType type;
        public string spelling;
        public SourcePosition position;

        public Token(TokenType type, string spelling, SourcePosition position)
        {
            if (type == TokenType.Identifier)
            {
                TokenType CurrentType = FirstReservedWord;
                bool Searching = true;

                while (Searching)
                {
                    int Comparison = TokenTable[(int)CurrentType].CompareTo(spelling);
                    if (Comparison == 0)
                    {
                        this.type = CurrentType;
                        Searching = false;
                    }
                    else if (Comparison > 0 || CurrentType == LastReservedWord)
                    {
                        this.type = Token.TokenType.Identifier;
                        Searching = false;
                    }
                    else
                    {
                        CurrentType++;
                    }
                }

            }
            else
            {
                this.type = type;
            }
            this.spelling = spelling;
            this.position = position;
        }


        public static String Spell(TokenType Type)
        {
            return TokenTable[(int)Type];
        }

        public override String ToString()
        {
            return "Kind=" + type + ", spelling=" + spelling +
              ", position=" + position;
        }

        public enum TokenType
        {
            Identifier,
            IntegerLiteral,
            Operator,
            UnitFunction,
            UnitStatName,
            GridStatName,
            MaximaStatName,

            //-----ALPHEBETICAL BEGIN-----
            Attack,
            AttackSpeed,
            Behaviour,
            Config,
            Damage,
            Distance,
            Else,
            Grid,
            Health,
            Height,
            If,
            Maxima,
            Melee,
            MoveAway,
            Movement,
            MoveTowards,
            Position,
            Range,
            Ranged,
            Regiment,
            RegimentPosition,
            Regiments,
			Rules,
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
            Dot,
            Slash,
            LeftParen,
            RightParen,
            LeftBracket,
            RightBracket,
            Comma,
            SemiColon,
            Assignment,

            //Special dudes
            EndOfText,
            Error
        }
        private static string[] TokenTable = new string[]
        {
            "<identifier>",
            "<integer-literal>",
            "<operator>",
            "<unitfunction>", //Attack, MoveTowards, MoveAway = Tokens, eller?
            "<unitstatname>",
            "<gridstatname>",
            "<maximastatname>",
                //   ALPHABETICAL START
            "Attack",
            "AttackSpeed",
            "Behaviour",
            "Config",
            "Damage",
            "Distance",
            "else",
            "Grid",
            "Health",
            "Height",
            "if",
            "Maxima",
            "Melee",
            "MoveAway",
            "Movement",
            "MoveTowards",
            "Position",
            "Range",
            "Ranged",
            "Regiment",
            "RegimentPosition",
            "Regiments",
			"Rules",
            "SearchForEnemies",
            "SearchForFriends",
            "Size", 
            "Standards",
            "Team",
            "Teams",
            "Type",
            "while",
            "Width",
                //   ALPHABETICAL END  
            //Signs
            ".",
            "/",
            "(",
            ")",
            "{",
            "}",
            ",",
            ";",
            "=",
            "",
            "<error>"
        };
        //OBS: HVIS ATTACK, MOVETOWARDS, MOVEAWAY SKAL MED I TOKENTABLE SKAL FIRSTRESERVED ÆNDRES TIL ATTACK
        private readonly static TokenType FirstReservedWord = Token.TokenType.Attack,
                      LastReservedWord = Token.TokenType.Width;
        // OBS: DET ER HURTIGERE AT CASTE TIL INTS HER END INDE I WHILE LOOPS, JFV. MINI-TRIANGLE
    }
}
