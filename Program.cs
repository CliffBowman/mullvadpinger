using System;
using CommandLine;

namespace MullvadPinger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ParseCommandLineArgs(args);
        }

        private static void ParseCommandLineArgs(string[] args)
        {
            Parser.Default
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(clo =>
                {
                    //todo
                });
        }
    }
}
