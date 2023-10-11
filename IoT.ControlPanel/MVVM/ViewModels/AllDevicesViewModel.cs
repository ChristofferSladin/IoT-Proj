
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Models;
using IoT.ControlPanel.MVVM.Views;
using IoT.ControlPanel.Services;
using MvvmHelpers.Commands;
using System.Diagnostics;
using System.Windows.Input;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class AllDevicesViewModel : ObservableObject
{
    private IotHubManager _iotHubManager;
    private DeviceItem _deviceItem;
    public ICommand DeleteDeviceCommand { get; }

    public AllDevicesViewModel(DeviceItem deviceItem, IotHubManager iotHubManager)
    {
        _deviceItem = deviceItem;
        Location = deviceItem.Location ?? "";
        IsActive = deviceItem.IsActive;
        Icon = SetDeviceIcon();

        _iotHubManager = iotHubManager;

        DeleteDeviceCommand = new AsyncCommand(DeleteDeviceAsync);
    }

    private async Task DeleteDeviceAsync()
    {
        try
        {
            bool userConfirmed = await Application.Current.MainPage.DisplayAlert(
           "Delete Device",
           $"Are you sure you want to delete the device {DeviceId}?",
           "Yes",
           "No"
           );
            if (userConfirmed)
            {
                var success = await _iotHubManager.DeleteDeviceAsync(DeviceId);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Success",
                        "Successfully deleted the device.",
                        "OK"
                    );
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Failed to delete the device.",
                        "OK"
                    );
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
    }

    public string DeviceId => _deviceItem.DeviceId ?? "";
    public string DeviceType => _deviceItem.DeviceType ?? "";
    public string Vendor => _deviceItem.Vendor ?? "";

    [ObservableProperty]
    string location;

    [ObservableProperty]
    bool isActive;

    [ObservableProperty]
    string icon;

    private string SetDeviceIcon()
    {
        return DeviceType.ToLower() switch
        {
            "light" => "\uf0eb",
            _ => "\uf2db",
        };
    }

    [RelayCommand]
    async static Task BackToHome() => await Shell.Current.GoToAsync(nameof(HomePage));
}
