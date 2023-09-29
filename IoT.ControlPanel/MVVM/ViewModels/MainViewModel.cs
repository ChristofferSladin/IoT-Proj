using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Pages;
using IoT.ControlPanel.Services;
using System.Windows.Input;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private string _connectionStatusText;
    public string ConnectionStatusText
    {
        get => _connectionStatusText;
        set => SetProperty(ref _connectionStatusText, value);
    }

    private bool _isConnectionStatusVisible;
    public bool IsConnectionStatusVisible
    {
        get => _isConnectionStatusVisible;
        set => SetProperty(ref _isConnectionStatusVisible, value);
    }

    private bool _isDeviceConnected;
    public bool IsDeviceConnected
    {
        get => _isDeviceConnected;
        set => SetProperty(ref _isDeviceConnected, value);
    }

    public ICommand ToggleStateCommand { get; private set; }

    public MainViewModel()
    {
        ToggleStateCommand = new RelayCommand<ToggledEventArgs>(ToggleState);
    }

    [ObservableProperty]
    bool isConfigured;

    public async void ToggleState(ToggledEventArgs e)
    {
        bool isToggled = e.Value;
        string deviceId = "Device.Fan";
        string methodName = isToggled ? "start" : "stop";
        try
        {
            await AzureIoTHubService.SendDirectMethodAsync(deviceId, methodName);
            IsDeviceConnected = isToggled;  // Only update if successful
        }
        catch (Microsoft.Azure.Devices.Common.Exceptions.DeviceNotFoundException)
        {
            IsDeviceConnected = false; // Reset to off if not successful
            ConnectionStatusText = "Device Not Connected";
            IsConnectionStatusVisible = true;

            await Task.Delay(3000);

            IsConnectionStatusVisible = false;
        }
    }

    [RelayCommand]
    async Task GotoSettings() => await Shell.Current.GoToAsync(nameof(SettingsPage));

    [RelayCommand]
    async Task GoToAllDevices() => await Shell.Current.GoToAsync(nameof(AllDevicesPage));
}
