using CommandLine;

namespace MullvadPinger.model
{
    public class CommandLineOptions : MullvadVPNServer
    {
        [Option("parallel", Required = false, HelpText = "Numbers of servers to ping in parallel.")]
        public int NumbersServersToPingInParallel { get; set; } = 5;
    }
}
