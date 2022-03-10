using System.Text.Json.Serialization;
using CommandLine;

namespace MullvadPinger.model
{
    public class MullvadVPNServer
    {
        [JsonPropertyName("hostname")]
        [Option("hostname", Required = false, HelpText = "Ping servers that match or partially match the host name.")]
        public string? Hostname { get; set; }

        [JsonPropertyName("country_code")]
        [Option("country-code", Required = false, HelpText = "Ping servers that match or partially match the country code.")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        [Option("country-name", Required = false, HelpText = "Ping servers that match or partially match the country name.")]
        public string? CountryName { get; set; }

        [JsonPropertyName("city_code")]
        [Option("city-code", Required = false, HelpText = "Ping servers that match or partially match the city code.")]
        public string? CityCode { get; set; }

        [JsonPropertyName("city_name")]
        [Option("city-name", Required = false, HelpText = "Ping servers that match or partially match the city name.")]
        public string? CityName { get; set; }

        [JsonPropertyName("active")]
        [Option("active", Required = false, HelpText = "Ping servers that are currently active (i.e. not down).")]
        public bool? IsActive { get; set; }

        [JsonPropertyName("owned")]
        [Option("owned", Required = false, HelpText = "Ping servers that are owned and not rented by Mullvad.")]
        public bool? IsOwned { get; set; }

        [JsonPropertyName("provider")]
        public string? Provider { get; set; }

        [JsonPropertyName("ipv4_addr_in")]
        public string? IPV4 { get; set; }

        [JsonPropertyName("ipv6_addr_in")]
        public string? IPV6 { get; set; }

        [JsonPropertyName("network_port_speed")]
        [Option("network-port-speed", Required = false, HelpText = "Ping servers that match server bandwidth speed in GB.")]
        public int? SpeedInGbps { get; set; }

        [JsonPropertyName("type")]
        [Option("type", Required = false, HelpText = "Ping servers that match server type.")]
        public string? ServerType { get; set; }

        [JsonPropertyName("status_messages")]
        public string[]? StatusMessages { get; set; }

        [JsonPropertyName("pubkey")]
        public string? PublicKey { get; set; }

        [JsonPropertyName("multihop_port")]
        public int? MultiHopPort { get; set; }

        [JsonPropertyName("socks_name")]
        public string? SocksServer { get; set; }
    }
}
