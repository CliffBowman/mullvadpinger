namespace MullvadPinger.model
{
    public record class PingResult
    {
        public string? Hostname { get; init; }
        public string? IPAddress { get; init; }
        public long MinRate { get; init; }
        public double AverageRate { get; init; }
        public long MaxRate { get; init; }
    }
}
