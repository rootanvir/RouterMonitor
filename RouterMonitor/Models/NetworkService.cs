using RouterMonitor.Models;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RouterMonitor.Services
{
    public class NetworkService
    {
        public RouterInfo GetRouterInfo()
        {
            RouterInfo info = new();
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up) continue;

                var props = ni.GetIPProperties();
                var gateway = props.GatewayAddresses
                    .FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork);
                if (gateway == null) continue;

                var ip4 = props.UnicastAddresses.FirstOrDefault(
                    a => a.Address.AddressFamily == AddressFamily.InterNetwork);
                info.GatewayIP = gateway.Address.ToString();
                info.LocalIP = ip4?.Address.ToString();
                info.SubnetMask = ip4?.IPv4Mask.ToString();

                info.DnsServers = string.Join(", ", props.DnsAddresses
                    .Where(d => d.AddressFamily == AddressFamily.InterNetwork)
                    .Select(d => d.ToString()));
                info.AdapterName = ni.Name;
                info.AdapterDescription = ni.Description;
                info.MacAddress = string.Join(":", ni.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));

                break;

            }

            GetWifiDetails(info);

            return info;
        }
        private void GetWifiDetails(RouterInfo info)
        {
            ProcessStartInfo psi = new()
            {
                FileName = "netsh",
                Arguments = "wlan show interfaces",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();

            foreach (string line in output.Split('\n'))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("SSID") &&
                    !trimmed.StartsWith("SSID name") &&
                    !trimmed.StartsWith("SSID BSSID"))
                {
                    info.SSID = GetValue(trimmed);
                }
                else if (trimmed.StartsWith("Signal"))
                {
                    info.SignalStrength = GetValue(trimmed);
                }
                else if (trimmed.StartsWith("Signal"))
                {
                    info.SignalStrength = GetValue(trimmed);
                }
                else if (trimmed.StartsWith("Channel"))
                {
                    info.Channel = GetValue(trimmed);
                }
                else if (trimmed.StartsWith("Receive rate"))
                {
                    info.RecieveRate = GetValue(trimmed);
                }
                else if (trimmed.StartsWith("Transmit rate"))
                {
                    info.TransmitRate = GetValue(trimmed);
                }
            }

        }
        private string GetValue(string line)
        {
            int idx = line.IndexOf(':');
            if (idx < 0) return "";
            return line[(idx + 1)..].Trim();
        }
    }
}