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
        }

    }
}