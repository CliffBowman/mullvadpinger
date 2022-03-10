using CommandLine;

namespace MullvadPinger.model
{
    public class CommandLineOptions : MullvadVPNServer
    {
        [Option("parallel", Required = false, HelpText = "Numbers of servers to ping in parallel.", Default = 5)]
        public int NumbersServersToPingInParallel { get; set; }

        [Option("interval", Required = false, HelpText = "Delay in milliseconds between pinging the same server (to prevent ping flood).", Default = 200)]
        public int PingIntervalMS { get; set; }

        [Option("pings-per-server", Required = false, HelpText = "Numbers of times to ping a server.", Default = 5)]
        public int PingsPerServer { get; set; }
    }
}
