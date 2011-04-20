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
    class Scanner
    {

        public Scanner();

        public bool IsGraphic(Char c);

        public void ScanComment();
        public TokenType Scan();
    }
}
