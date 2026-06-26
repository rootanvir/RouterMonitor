using System.Windows;
using RouterMonitor.Services;

namespace RouterMonitor
{
    public partial class MainWindow : Window
    {
        private readonly NetworkService networkService;

        public MainWindow()
        {
            InitializeComponent();
            networkService = new NetworkService();
        }
        private void GetRouterInfo_Click(object sender, RoutedEventArgs e)
        {
            var router = networkService.GetRouterInfo();

            MessageBox.Show(
                $"SSID: {router.SSID}\n" +
                $"Gateway: {router.GatewayIP}\n" +
                $"Local IP: {router.LocalIP}\n" +
                $"Subnet: {router.SubnetMask}\n" +
                $"DNS: {router.DnsServers}\n" +
                $"Adapter: {router.AdapterName}\n" +
                $"MAC: {router.MacAddress}\n" +
                $"Signal: {router.SignalStrength}\n" +
                $"Channel: {router.Channel}\n" +
                $"RX Rate: {router.RecieveRate}\n" +
                $"TX Rate: {router.TransmitRate}"
            );
        }

    }
}