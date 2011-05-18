using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    
    public class Scanner
    {
        private Source SourceFile;
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
        
        private void ScanSeparator()
        {
            switch (CurrentChar)
            {
                case '/':
                    {
                        // A small check on / to see if the next character is also // to handle comments.
                        if (CurrentChar == '/' && SourceFile.PeekSource() == '/')
                        {
                            TakeIt();
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

        private Token.TokenType ScanToken()
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
                    return Token.TokenType.Identifier;

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
                    return Token.TokenType.IntegerLiteral;

                
                case '=': //The following is used to ensure that the scanner knows the difference between an operator and an assignment. 
                    TakeIt();
                    if (CurrentChar == '=')
                    {
                        TakeIt();
                        return Token.TokenType.Operator;
                    }
                    else
                    {
                        return Token.TokenType.Assignment;
                    }
                case '+':
                case '-':
                case '*':
                case '/':
                case '<':
                case '>':
                case '|':
                case '&':
                    TakeIt();
                    while (IsOperator(CurrentChar))
                        TakeIt();
                    return Token.TokenType.Operator;

              
                case ';':
                    TakeIt();
                    return Token.TokenType.SemiColon;

                case ',':
                    TakeIt();
                    return Token.TokenType.Comma;

                case '(':
                    TakeIt();
                    return Token.TokenType.LeftParen;

                case ')':
                    TakeIt();
                    return Token.TokenType.RightParen;

                case '{':
                    TakeIt();
                    return Token.TokenType.LeftBracket;

                case '}':
                    TakeIt();
                    return Token.TokenType.RightBracket;

                case '.':
                    TakeIt();
                    return Token.TokenType.Dot;
                case Source.EndOfText:
                    return Token.TokenType.EndOfText;

                default:
                    TakeIt();
                    return Token.TokenType.Error ;
            }
        }
        public Token Scan()
        {
            Token tok;
            SourcePosition pos;
            Token.TokenType type;

            CurrentlyScanningToken = false;
            while ((CurrentChar == '/' && SourceFile.PeekSource() == '/') ||
                    CurrentChar == ' ' ||
                    CurrentChar == '\n' ||
                    CurrentChar == '\r' ||
                    CurrentChar == '\t')
            {
                ScanSeparator();
            }
            CurrentlyScanningToken = true;
            CurrentSpelling = new StringBuilder("");
            pos = new SourcePosition();
            pos.start = SourceFile.GetCurrentLine();
            type = ScanToken();

            pos.finish = SourceFile.GetCurrentLine();
            tok = new Token(type, CurrentSpelling.ToString(), pos);
            //if (debug)
            //    Console.WriteLine(tok);
            return tok;
        }

    }
}
