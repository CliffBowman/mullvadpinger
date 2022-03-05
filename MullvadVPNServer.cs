namespace MullvadPinger
{
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
