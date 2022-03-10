using System.Linq.Expressions;
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

        public async Task<List<MullvadVPNServer>> GetVPNServerListAsync(MullvadVPNServer? filter = null)
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
                query = FilterString(query, nameof(filter.Hostname), filter.Hostname);

            if (filter.CountryCode != null)
                query = FilterString(query, nameof(filter.CountryCode), filter.CountryCode);

            if (filter.CountryName != null)
                query = FilterString(query, nameof(filter.CountryName), filter.CountryName);

            if (filter.CityCode != null)
                query = FilterString(query, nameof(filter.CityCode), filter.CityCode);

            if (filter.CityName != null)
                query = FilterString(query, nameof(filter.CityName), filter.CityName);

            if (filter.IsActive != null)
                query = FilterBoolean(query, nameof(filter.IsActive), filter.IsActive);

            if (filter.IsOwned != null)
                query = FilterBoolean(query, nameof(filter.IsOwned), filter.IsOwned);

            if (filter.Provider != null)
                query = FilterString(query, nameof(filter.Provider), filter.Provider);

            if (filter.IPV4 != null)
                query = FilterString(query, nameof(filter.IPV4), filter.IPV4);

            if (filter.IPV6 != null)
                query = FilterString(query, nameof(filter.IPV6), filter.IPV6);

            if (filter.SpeedInGbps != null)
                query = FilterInt(query, nameof(filter.SpeedInGbps), filter.SpeedInGbps);

            if (filter.ServerType != null)
                query = FilterString(query, nameof(filter.ServerType), filter.ServerType);

            if (filter.PublicKey != null)
                query = FilterString(query, nameof(filter.PublicKey), filter.PublicKey);

            if (filter.MultiHopPort != null)
                query = FilterInt(query, nameof(filter.MultiHopPort), filter.MultiHopPort);

            if (filter.SocksServer != null)
                query = FilterString(query, nameof(filter.SocksServer), filter.SocksServer);

            return query.ToList();
        }

        private IQueryable<MullvadVPNServer> FilterString(IQueryable<MullvadVPNServer> query, string propertyName, string filterValue)
        {
            var x = Expression.Parameter(typeof(MullvadVPNServer), "x");
            var property = Expression.Property(x, propertyName);
            var notNull = Expression.NotEqual(property, Expression.Constant(null));
            var contains = Expression.Call(property, "Contains", null, Expression.Constant(filterValue), Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
            var lambda = Expression.Lambda<Func<MullvadVPNServer, bool>>(Expression.And(notNull, contains), x);
            return query.Where(lambda);
        }

        private IQueryable<MullvadVPNServer> FilterBoolean(IQueryable<MullvadVPNServer> query, string propertyName, bool? filterValue)
        {
            var x = Expression.Parameter(typeof(MullvadVPNServer), "x");
            var property = Expression.Property(x, propertyName);
            var lambda = Expression.Lambda<Func<MullvadVPNServer, bool>>(Expression.Equal(property, Expression.Constant(filterValue, typeof(bool?))), x);
            return query.Where(lambda);
        }

        private IQueryable<MullvadVPNServer> FilterInt(IQueryable<MullvadVPNServer> query, string propertyName, int? filterValue)
        {
            var x = Expression.Parameter(typeof(MullvadVPNServer), "x");
            var property = Expression.Property(x, propertyName);
            var lambda = Expression.Lambda<Func<MullvadVPNServer, bool>>(Expression.Equal(property, Expression.Constant(filterValue, typeof(int?))), x);
            return query.Where(lambda);
        }
    }
}
