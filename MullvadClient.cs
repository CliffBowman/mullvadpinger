using System.Text.Json;
using Microsoft.Extensions.Logging;
using MullvadPinger.model;

namespace MullvadPinger
{
    public class MullvadClient : IMullvadClient
    {
        private readonly IMullvadDataSource _mullvadDataSource;
        private readonly ILogger<MullvadClient> _logger;

        public MullvadClient(IMullvadDataSource mullvadDataSource, ILogger<MullvadClient> logger)
        {
            this._mullvadDataSource = mullvadDataSource;
            this._logger = logger;
        }

        public async Task<List<MullvadVPNServer>> GetVPNServerListAsync(VPNServerListFilter? filter = null)
        {
            var serverJsonString = await _mullvadDataSource.GetVPNServerJsonAsync();

            List<MullvadVPNServer>? servers;

            try
            {
                servers = JsonSerializer.Deserialize<List<MullvadVPNServer>>(serverJsonString);
            }
            catch (JsonException je)
            {
                _logger.LogError(je, "Error occurred while deserializing response from Mullvad API.");
                throw;
            }

            if (servers == null)
            {
                _logger.LogError("Error retrieving Mullvad server list.");
                throw new Exception("Error retrieving Mullvad server list.");
            }

            if (filter == null)
                return servers;

            var query = servers.AsQueryable();

            if (filter.Hostname != null)
                query = query.Where(s => s.Hostname != null && s.Hostname.Contains(filter.Hostname, StringComparison.CurrentCultureIgnoreCase));

            if (filter.CountryCode != null)
                query = query.Where(s => s.CountryCode != null && s.CountryCode.Contains(filter.CountryCode, StringComparison.CurrentCultureIgnoreCase));

            // TODO: Add additional filtering.

            return query.ToList();
        }
    }
}
