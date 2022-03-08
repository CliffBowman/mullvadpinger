namespace MullvadPinger
{
    public class MullvadDataSource : IMullvadDataSource
    {
        private readonly string serverListUrl = "https://api.mullvad.net/www/relays/all/";

        public async Task<string> GetVPNServerJsonAsync()
        {
            using (var client = new HttpClient())
                return await client.GetStringAsync(serverListUrl);
        }
    }

    public class SampleMullvadDataSource : IMullvadDataSource
    {
        private readonly string filename = "data/mullvad-www-relays-all-response-sample.json";

        public async Task<string> GetVPNServerJsonAsync()
        {
            return await File.ReadAllTextAsync(filename);
        }
    }
}
