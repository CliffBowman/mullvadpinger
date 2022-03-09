using System.Text.Json.Serialization;

namespace MullvadPinger.model
{
    public class MullvadVPNServer
    {
        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }
        [JsonPropertyName("country_name")]
        public string? CountyName { get; set; }
        [JsonPropertyName("city_code")]
        public string? CityCode { get; set; }
        [JsonPropertyName("city_name")]
        public string? CityName { get; set; }
        [JsonPropertyName("active")]
        public bool? IsActive { get; set; }
        [JsonPropertyName("owned")]
        public bool? IsOwned { get; set; }
        [JsonPropertyName("provider")]
        public string? Provider { get; set; }
        [JsonPropertyName("ipv4_addr_in")]
        public string? IPV4 { get; set; }
        [JsonPropertyName("ipv6_addr_in")]
        public string? IPV6 { get; set; }
        [JsonPropertyName("network_port_speed")]
        public int? SpeedInGbps { get; set; }
        [JsonPropertyName("type")]
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
