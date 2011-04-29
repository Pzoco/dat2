using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    
    class Scanner
    {
        private Source SourceFile;
        private char PreviousChar;
        private char CurrentChar;
        private StringBuilder CurrentSpelling;
        private bool CurrentlyScanningToken;
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
        public Scanner(Source source)
        {
            SourceFile = source;
            CurrentChar = SourceFile.GetSource();
        }
        private void TakeIt()
        {
            if (CurrentlyScanningToken)
            {
                CurrentSpelling.Append(CurrentChar);
            }
            CurrentChar = SourceFile.GetSource();
        }
        // OBS: HER ER FARE FOR FEJL: DER SKAL SCANNES EN SEPERATOR FOR COMMENTS '//' MEN CURRENTCHAR HOLDER KUN ET SYMBOL AF GANGEN 
        private void ScanSeparator()
        {
            switch (CurrentChar)
            {
                case '/':
                    {
                        PreviousChar = CurrentChar;
                        TakeIt();
                        if (CurrentChar == '/' && PreviousChar == '/')
                        {
                            while ((CurrentChar != Source.EndOfLine) && (CurrentChar != Source.EndOfText))
                            {
                                TakeIt();
                            }
                            if (CurrentChar == Source.EndOfLine)
                            {
                                TakeIt();
                            }
                        }
                    }
                    break;

                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    TakeIt();
                    break;
            }
        }

        private int ScanToken()
        {

            switch (CurrentChar)
            {

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    TakeIt();
                    while (IsLetter(CurrentChar) || IsDigit(CurrentChar))
                        TakeIt();
                    return (int)Token.Tokens.IDENTIFIER;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    TakeIt();
                    while (IsDigit(CurrentChar))
                        TakeIt();
                    return (int)Token.Tokens.INTLITERAL;

                case '+':
                case '-':
                case '*':
                case '/':
                case '=':
                case '<':
                case '>':
                case '\\':
                case '&':
                case '@':
                case '%':
                case '^':
                case '?':
                    TakeIt();
                    while (IsOperator(CurrentChar))
                        TakeIt();
                    return (int)Token.Tokens.OPERATOR;

                case '\'':
                    TakeIt();
                    TakeIt(); // the quoted character
                    if (CurrentChar == '\'')
                    {
                        TakeIt();
                        return (int)Token.Tokens.CHARLITERAL;
                    }
                    else
                        return (int)Token.Tokens.ERROR;

                case '.':
                    TakeIt();
                    return (int)Token.Tokens.DOT;

                case ':':
                    TakeIt();
                    if (CurrentChar == '=')
                    {
                        TakeIt();
                        return (int)Token.Tokens.BECOMES;
                    }
                    else
                        return (int)Token.Tokens.COLON;

                case ';':
                    TakeIt();
                    return (int)Token.Tokens.SEMICOLON;

                case ',':
                    TakeIt();
                    return (int)Token.Tokens.COMMA;

                case '~':
                    TakeIt();
                    return (int)Token.Tokens.IS;

                case '(':
                    TakeIt();
                    return (int)Token.Tokens.LPAREN;

                case ')':
                    TakeIt();
                    return (int)Token.Tokens.RPAREN;

                case '[':
                    TakeIt();
                    return (int)Token.Tokens.LBRACKET;

                case ']':
                    TakeIt();
                    return (int)Token.Tokens.RBRACKET;

                case '{':
                    TakeIt();
                    return (int)Token.Tokens.LCURLY;

                case '}':
                    TakeIt();
                    return (int)Token.Tokens.RCURLY;

                case Source.EndOfText:
                    return (int)Token.Tokens.EndOfText;

                default:
                    TakeIt();
                    return (int)Token.Tokens.ERROR;
            }
        }
    }
}
