using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class Source
    {
        public const char EndOfLine = '\n';
        public const char EndOfText = '\u0000';

        System.IO.FileInfo sourceFile;
        System.IO.StreamReader source;
        int currentLine;

        public Source(String filename)
        {
            try
            {
                sourceFile = new System.IO.FileInfo(filename);
                source = sourceFile.OpenText();
                currentLine = 1;
            }
            catch
            {
                sourceFile = null;
                source = null;
                currentLine = 0;
            }
            
        }
        internal char GetSource()
        {
            try
            {
                int c = source.Read();

                if (c == -1)
                {
                    c = EndOfText;
                }
                else if (c == EndOfLine)
                {
                    currentLine++;
                }
                return (char)c;
            }
            catch (System.IO.IOException s)
            {
                return EndOfText;
            }
        }
        internal char PeekSource()
        {
            try
            {
                int c = source.Peek();

                if (c == -1)
                {
                    c = EndOfText;
                }
                return (char)c;
            }
            catch (System.IO.IOException s)
            {
                return EndOfText;
            }
        }
        internal int GetCurrentLine()
        {
            return currentLine;
        }
    }

    public class SourcePosition
    {

        public int start, finish;

        public SourcePosition()
        {
            start = 0;
            finish = 0;
        }

        public SourcePosition(int s, int f)
        {
            start = s;
            finish = f;
        }

        public String ToString()
        {
            return "(" + start + ", " + finish + ")";
        }
    }
}
