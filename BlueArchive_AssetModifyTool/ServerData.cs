namespace ExcelExtractor
{
    internal class OverrideConnectionGroup
    {
        public string Name { get; set; }
        public string AddressablesCatalogUrlRoot { get; set; }
    }

    internal class ConnectionGroup
    {
        public string Name { get; set; }
        public string ManagementDataUrl { get; set; }
        public bool IsProductionAddressables { get; set; }
        public string ApiUrl { get; set; }
        public string GatewayUrl { get; set; }
        public string KibanaLogUrl { get; set; }
        public string ProhibitedWordBlackListUri { get; set; }
        public string ProhibitedWordWhiteListUri { get; set; }
        public string CustomerServiceUrl { get; set; }
        public OverrideConnectionGroup[] OverrideConnectionGroups { get; set; }
        public string BundleVersion { get; set; }
    }

    internal class ServerData
    {
        public ConnectionGroup[] ConnectionGroups { get; set; }
    }
}
