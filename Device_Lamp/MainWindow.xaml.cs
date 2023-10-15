using Device_Lamp.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Device_Lamp;

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

        Task.WhenAll(ToggleLampStateAsync(), CheckConnectivityAsync());
    }

    private bool _isLampOn = false;

    private async Task ToggleLampStateAsync()
    {
        Storyboard lampOn = (Storyboard)this.FindResource("LampOnStoryboard");
        Storyboard lampOff = (Storyboard)this.FindResource("LampOffStoryboard");

        while (true)
        {
            bool shouldLampBeOn = _deviceManager.AllowSending();
            if (shouldLampBeOn != _isLampOn)
            {
                _isLampOn = shouldLampBeOn;
                if (_isLampOn)
                {
                    lampOn.Begin();
                }
                else
                {
                    lampOff.Begin();
                }
            }

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
