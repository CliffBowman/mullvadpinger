using MullvadPinger.model;

namespace MullvadPinger
{
    public interface IMullvadClient
    {
        Task<List<MullvadVPNServer>> GetVPNServerListAsync(MullvadVPNServer? filter = null);
    }
}
