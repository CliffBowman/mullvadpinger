using MullvadPinger.model;

namespace MullvadPinger
{
    public interface IMullvadClient
    {
        Task<List<MullvadVPNServer>> GetVPNServerListAsync(VPNServerListFilter? filter = null);
    }
}
