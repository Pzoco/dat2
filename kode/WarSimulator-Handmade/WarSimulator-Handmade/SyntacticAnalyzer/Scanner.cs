using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    
    class Scanner
    {
        private char CurrentChar;
        private StringBuilder CurrentSpelling;

        private bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        private bool IsOperator(char c)
        {
         return (c == '+' || c == '-' || c == '*' || c == '/' || c == '<' || c == '>' || c == '=' || c == '&' || c == '|');
        }
        private void take(char ExpectedChar)
        {
            if (CurrentChar == ExpectedChar)
            {
                CurrentSpelling.Append(CurrentChar);
            }

        }
        private void takeIt()
        {
            
        }
    }
}
