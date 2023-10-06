using System;
using System.Windows;
using Device_Fan.Services;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Device_Fan
{
    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;
        private readonly NetworkManager _networkManager;

        public MainWindow(DeviceManager deviceManager, NetworkManager networkManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _networkManager = networkManager;

            ConnectivityStatus.Text = "Connected";
            ConnectivityStatus.Foreground = Brushes.LawnGreen;

            Task.WhenAll(ToggleFanStateAsync(), CheckConnectivityAsync());
        }

        private async Task ToggleFanStateAsync()
        {
            Storyboard fan = (Storyboard)this.FindResource("FanStoryBoard");

            while (true)
            {
                if (_deviceManager.AllowSending())
                {
                    fan.Begin();
                }
                else fan.Stop();

                await Task.Delay(1000);
            }
        }

        private async Task CheckConnectivityAsync()
        {
            while (true)
            {
                ConnectivityStatus.Text = await _networkManager.CheckConnectivityAsync();
                await Task.Delay(1000);
            }
        }
    }
}
