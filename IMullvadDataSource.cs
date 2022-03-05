namespace MullvadPinger
{
    public interface IMullvadDataSource
    {
        Task<string> GetVPNServerJsonAsync();
    }
}
