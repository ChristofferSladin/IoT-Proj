using Device_SmartLock.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Device_SmartLock;

public partial class MainWindow : Window
{
    private readonly DeviceManager _deviceManager;
    private readonly NetworkManager _networkManager;
    private bool _isLocked = true;

    public MainWindow(DeviceManager deviceManager, NetworkManager networkManager)
    {
        InitializeComponent();

        _deviceManager = deviceManager;
        _networkManager = networkManager;

        ConnectivityStatus.Text = "Connected";
        ConnectivityStatus.Foreground = System.Windows.Media.Brushes.LawnGreen;

        Task.WhenAll(ToggleLockStateAsync(), CheckConnectivityAsync());
    }

    private async Task ToggleLockStateAsync()
    {
        Storyboard lockOpen = (Storyboard)this.FindResource("LockOpenStoryboard");
        Storyboard lockClosed = (Storyboard)this.FindResource("LockClosedStoryboard");

        while (true)
        {
            bool shouldLockBeClosed = _deviceManager.AllowSending();
            if (shouldLockBeClosed != _isLocked)
            {
                _isLocked = shouldLockBeClosed;
                if (_isLocked)
                {
                    LockIcon.Text = "\uf023";
                    lockClosed.Begin();
                }
                else
                {
                    LockIcon.Text = "\uf09c";
                    lockOpen.Begin();
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

