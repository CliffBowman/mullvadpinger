using System.Linq.Expressions;
using System.Reflection;
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
            var type = new MullvadVPNServer();

            if (filter.Hostname != null)
                query = FilterString(query, nameof(type.Hostname), filter.Hostname);

            if (filter.CountryCode != null)
                query = FilterString(query, nameof(type.CountryCode), filter.CountryCode);

            if (filter.CountryName != null)
                query = FilterString(query, nameof(type.CountryName), filter.CountryName);

            if (filter.CityCode != null)
                query = FilterString(query, nameof(type.CityCode), filter.CityCode);

            if (filter.CityName != null)
                query = FilterString(query, nameof(type.CityName), filter.CityName);

            if (filter.Active != null)
                query = FilterBoolean(query, nameof(type.IsActive), filter.Active.Value);

            if (filter.Owned != null)
                query = FilterBoolean(query, nameof(type.IsOwned), filter.Owned.Value);

            // TODO: Add additional filtering.

            return query.ToList();
        }

        private IQueryable<MullvadVPNServer> FilterString(IQueryable<MullvadVPNServer> query, string propertyName, string filterValue)
        {
            var x = Expression.Parameter(typeof(MullvadVPNServer), "x");
            var property = Expression.Property(x, propertyName);
            var notNull = Expression.NotEqual(property, Expression.Constant(null));
            var contains = Expression.Call(property, "Contains", null, Expression.Constant(filterValue), Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
            var final = Expression.Lambda<Func<MullvadVPNServer, bool>>(Expression.And(notNull, contains), x);
            return query.Where(final);
        }

        private IQueryable<MullvadVPNServer> FilterBoolean(IQueryable<MullvadVPNServer> query, string propertyName, bool filterValue)
        {
            var x = Expression.Parameter(typeof(MullvadVPNServer), "x");
            var property = Expression.Property(x, propertyName);
            var final = Expression.Lambda<Func<MullvadVPNServer, bool>>(Expression.Equal(property, Expression.Constant(filterValue, typeof(bool?))), x);
            return query.Where(final);
        }

        private IQueryable<MullvadVPNServer> FilterInt(IQueryable<MullvadVPNServer> query, string propertyName, int filterValue)
        {
            var x = Expression.Parameter(typeof(MullvadVPNServer), "x");
            var property = Expression.Property(x, propertyName);
            var final = Expression.Lambda<Func<MullvadVPNServer, bool>>(Expression.Equal(property, Expression.Constant(filterValue, typeof(int?))), x);
            return query.Where(final);
        }
    }
}
