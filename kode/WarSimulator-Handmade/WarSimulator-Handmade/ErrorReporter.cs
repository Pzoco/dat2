using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class ErrorReporter
    {
        public int numErrors;

        public ErrorReporter()
        {
            numErrors = 0;
        }

        public void ReportError(String message, String tokenName, SourcePosition pos)
        {
            Console.Write("ERROR: ");

            for (int p = 0; p < message.Length; p++)
                if (message[p] == '%')
                    Console.Write(tokenName);
                else
                    Console.Write(message[p]);
            Console.WriteLine(" " + pos.start + ".." + pos.finish);
            numErrors++;
        }

        public void ReportRestriction(String message)
        {
            Console.WriteLine("RESTRICTION: " + message);
        }
    }
}
