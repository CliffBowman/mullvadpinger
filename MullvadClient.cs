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
}
