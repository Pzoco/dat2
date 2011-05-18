using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class EntryPoint
    {
        private static Parser parser;
        private static Checker checker;
        private static ErrorReporter reporter;

        private static ConfigFile configFile;
        private static TeamFile[] teamFiles;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[2];
                args[0] = "TestData/config.cfg";
                args[1] = "TestData/team.war";
                TryStartSimulation(args);
            }
            else if (args.Length < 3)
            {
                Console.WriteLine("Invalid number of arguments - Correct usage: <configfile> <teamfile 1> <teamfile 2> ... <teamfile n>");
            }
            else
            {
                TryStartSimulation(args);
            }
            Console.WriteLine("Press Enter to exit");
            Console.ReadKey();
        }

        private static void TryStartSimulation(string[] args)
        {
            bool correctUsage = true;
            bool allowedToCheck = true;
            bool allowedToInteprete = true;

            #region Checking for Correct Usage
            List<string> teamFilesArgs = new List<string>();
            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);
            for (int i = 1; i < args.Length; i++)
            {
                if (!args[i].EndsWith(".war"))
                {
                    Console.WriteLine("Incorrect filename of teamfile at {0} - Correct usage: .war", i);
                    correctUsage = false;
                }
                teamFilesArgs.Add(args[i]);
            }
            if (!args[0].EndsWith(".cfg"))
            {
                Console.WriteLine("Incorrect filename of configfile - Correct usage: .cfg");
                correctUsage = false;
            }
            #endregion

            #region Parsing
            if (correctUsage)
            {
                //Parsing the configfile
                Console.WriteLine("Parsing the configFile");
                reporter = new ErrorReporter();
                parser = new Parser("TestData/config.cfg", reporter);
                configFile = parser.ParseConfigFile();
                if (reporter.numbErrors > 0) { allowedToCheck = false; }

                //Instantiating the teamfiles and parsing
                teamFiles = new TeamFile[args.Length - 1];
                for (int i = 0; i < args.Length-1; i++)
                {
                    reporter = new ErrorReporter();
                    Console.WriteLine("Parsing teamfile {0}", i);
                    parser = new Parser(args[i+1], reporter);
                    teamFiles[i] = parser.ParseTeamFile();
                    if (reporter.numbErrors > 0) { allowedToCheck = false; }
                }
            }
            else
            {
                return;
            }
            #endregion

            #region Contextual Analyzing
            if (allowedToCheck)
            {
                Console.WriteLine("Checking configfile ast");
                reporter = new ErrorReporter();
                checker = new Checker(configFile, reporter);
				checker.CheckConfigFile();

                for (int i = 0; i < args.Length-1; i++)
                {
                    Console.WriteLine("Checking teamfile ast {0}", i);
                    reporter = new ErrorReporter();
                    checker = new Checker(teamFiles[i], reporter);
					checker.CheckTeamFile();
                    if (reporter.numbErrors > 0)
                    {
                        allowedToInteprete = false;
                    }
                }
            }
            else
            {
                return;
            }
            #endregion

            #region Intepreting
            if (allowedToInteprete)
            {
                // Send all the data to the Simulator class A list of teamfiles and configfiles, + AST's
                Simulator simulator = new Simulator(configFile,teamFiles);
				simulator.Run();
            }
            else
            {
                return;
            }
            #endregion
        }

    }
}
