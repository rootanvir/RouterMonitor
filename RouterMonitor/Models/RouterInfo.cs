namespace RouterMonitor.Models
{
    public class RouterInfo
    {
        public string? GatewayIP { get; set; }
        public string? AdapterName { get; set; }
        public string? AdapterDescription { get; set; }
        public string? LocalIP { get; set; }
        public string? SubnetMask { get; set; }
        public string? DnsServers { get; set; }
        public string? MacAddress { get; set; }
    }
}