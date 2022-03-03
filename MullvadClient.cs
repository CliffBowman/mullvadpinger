using Newtonsoft.Json.Linq;

namespace MullvadPinger
{
    public class MullvadClient : IMullvadClient
    {
        private readonly IMullvadDataSource mullvadDataSource;
        private readonly string vpnServerUrlFormat = "{0}.mullvad.net";

        public MullvadClient(IMullvadDataSource mullvadDataSource)
        {
            this.mullvadDataSource = mullvadDataSource;
        }

        public async Task<List<MullvadVPNServer>> GetVPNServerListAsync()
        {
            var serverJsonString = await mullvadDataSource.GetVPNServerJsonAsync();
            var serverArray = JArray.Parse(serverJsonString);

            return serverArray
                .Select(server => new MullvadVPNServer
                {
                    Hostname = string.Format(vpnServerUrlFormat, (string?)server["hostname"]),
                    PublicKey = (string?)server["pubkey"],
                    SpeedInGbps = (int?)server["network_port_speed"] ?? -1,
                })
                .ToList();
        }
    }

    public interface IMullvadClient
    {
        Task<List<MullvadVPNServer>> GetVPNServerListAsync();
    }

    public record class MullvadVPNServer
    {
        public string? Hostname { get; init; }
        public string? PublicKey { get; init; }
        public int SpeedInGbps { get; init; }

        public override string ToString()
        {
            return $"{Hostname} {PublicKey} {SpeedInGbps}";
        }
    }
}
