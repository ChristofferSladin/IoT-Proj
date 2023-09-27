using SharedLibrary.Services;
using System.Threading.Tasks;
using System.Windows;

namespace Device.Lamp;

public partial class MainWindow : Window
{
    private readonly DeviceManager _deviceManager;
    private readonly NetworkManager _networkManager;
    public MainWindow(DeviceManager deviceManager, NetworkManager networkManager)
    {
        InitializeComponent();

        _deviceManager = deviceManager;
        _networkManager = networkManager;

        Task.FromResult(CheckConnectivityAsync);
    }

    private async Task CheckConnectivityAsync()
    {
        while (true)
        {
            ConnStatus.Text = await _networkManager.CheckConnectivityAsync();
            await Task.Delay(1000);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (InUse.Text == "OFF")
            InUse.Text = "ON";
        else
            InUse.Text = "OFF";
    }
}


