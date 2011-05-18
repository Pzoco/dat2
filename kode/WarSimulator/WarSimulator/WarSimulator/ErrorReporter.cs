using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class ErrorReporter
    {
        public int numbErrors;

        public ErrorReporter()
        {
            numbErrors = 0;
        }

        public void ReportParserError(String message, Token token)
        {
            String output;
            output =  "SYNTAX ERROR FOUND IN LINE " + token.position.ToString() + " WHILE TRYING TO PARSE TOKEN OF TYPE " + token.type.ToString() + ".";
            if (message.Length>1)
            {
                output += "\n" + message;
            }
            Console.WriteLine(output);
            numbErrors++;
        }
        // THIS IS A SIMPLE ERROR REPORTER, BORROWED FROM THE C# IMPLEMENTATION OF MINI-TRIANGLE MADE BY BENT THOMSEN
        public void ReportCheckerError(String msg, String tokName, SourcePosition pos)
        {
            Console.Write("CHECKER ERROR!");

            for (int i = 0; i < msg.Length; i++)
                if (msg[i] == '%')
                    Console.Write(tokName);
                else
                    Console.Write(msg[i]);
            Console.WriteLine(" " + pos.start + ".." + pos.finish);
            numbErrors++;
        }
    }
}
