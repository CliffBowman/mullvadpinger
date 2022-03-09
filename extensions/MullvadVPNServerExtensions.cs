namespace MullvadPinger.model
{
    public static class MullvadVPNServerExtensions
    {
        // TODO: look for override in appsettings.
        private static readonly string vpnServerUrlFormat = "{0}.mullvad.net";

        public static string FullyQualifiedHostname(this MullvadVPNServer server) => string.Format(vpnServerUrlFormat, server.Hostname);
    }
}
