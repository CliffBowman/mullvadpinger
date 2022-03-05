namespace MullvadPinger
{
    public interface IMullvadClient
    {
        Task<List<MullvadVPNServer>> GetVPNServerListAsync();
    }
}
