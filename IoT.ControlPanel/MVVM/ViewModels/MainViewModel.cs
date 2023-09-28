using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Pages;
using IoT.ControlPanel.Services;
using System.Windows.Input;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ICommand ToggleStateCommand { get; private set; }

    public MainViewModel()
    {
        ToggleStateCommand = new RelayCommand<ToggledEventArgs>(ToggleState);
    }

    public async void ToggleState(ToggledEventArgs e)
    {
        bool isToggled = e.Value;
        string deviceId = "Device.Fan";
        string methodName = isToggled ? "start" : "stop";

        // Call your Azure IoT Hub Service method
        await AzureIoTHubService.SendDirectMethodAsync(deviceId, methodName);
    }

    [RelayCommand]
    async Task GotoSettings() => await Shell.Current.GoToAsync(nameof(SettingsPage));

    [RelayCommand]
    async Task GoToAllDevices() => await Shell.Current.GoToAsync(nameof(AllDevicesPage));
}
