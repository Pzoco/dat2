using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class TestClass
    {
        static String objectName = "obj.tam";

        private static Scanner scanner;
        private static Parser parser;
        private static ErrorReporter reporter;
        private static TeamFile teamFile;
        private static ConfigFile configFile;
        private static Checker checker;

        static void Main(string[] args)
        {
            bool compiledOK;
            String sourceName;

            if (args.Length != 1)
            {
                sourceName = "C:\\Users\\Drfou\\Desktop\\kode.txt";
                //Console.WriteLine("Usage: tc filename");
                //Environment.Exit(1);
            }
            else
            {
                sourceName = args[0];
            }
            compiledOK = CompileProgram(sourceName, objectName);
            Console.ReadKey();
            Console.ReadLine();
        }
        static bool CompileProgram(String sourceName, String objectName)
        {
            Source source = new Source(sourceName);

            if (source == null)
            {
                Console.WriteLine("Can't access source file " + sourceName);
                Environment.Exit(1);
            }

            scanner = new Scanner(source);
            reporter = new ErrorReporter();
            parser = new Parser(scanner, reporter);

            teamFile = parser.ParseTeamFile();

            source = new Source("C:\\Users\\Drfou\\Desktop\\ConfigFile.txt");

            scanner = new Scanner(source);
            reporter = new ErrorReporter();
            parser = new Parser(scanner, reporter);

            configFile = parser.ParseConfigFile();
            if (reporter.numbErrors == 0)
            {
                Console.WriteLine("Parsing Completed Successfully!");
                checker.Check(teamFile);
                checker.Check(configFile);
            }
            else
            {
                Console.WriteLine("Parsing failed!");
            }
            return true;
        }
    }
}
