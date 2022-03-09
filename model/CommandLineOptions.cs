using CommandLine;

namespace MullvadPinger.model
{
    public class CommandLineOptions
    {
        [Option("hostname", Required = false, HelpText = "Ping servers that match or partially match the host name.")]
        public string? Hostname { get; set; }

        [Option("country-code", Required = false, HelpText = "Ping servers that match or partially match the country code.")]
        public string? CountryCode { get; set; }

        [Option("country-name", Required = false, HelpText = "Ping servers that match or partially match the country name.")]
        public string? CountryName { get; set; }

        [Option("city-code", Required = false, HelpText = "Ping servers that match or partially match the city code.")]
        public string? CityCode { get; set; }

        [Option("city-name", Required = false, HelpText = "Ping servers that match or partially match the city name.")]
        public string? CityName { get; set; }

        [Option("active", Required = false, HelpText = "Ping servers that are currently active (i.e. not down).")]
        public bool? Active { get; set; }

        [Option("owned", Required = false, HelpText = "Ping servers that are owned and not rented by Mullvad.")]
        public bool? Owned { get; set; }

        [Option("network-port-speed", Required = false, HelpText = "Ping servers that match server bandwidth speed in GB.")]
        public int? NetworkPortSpeed { get; set; }

        [Option("type", Required = false, HelpText = "Ping servers that match server type.")]
        public string? Type { get; set; }
    }
}
