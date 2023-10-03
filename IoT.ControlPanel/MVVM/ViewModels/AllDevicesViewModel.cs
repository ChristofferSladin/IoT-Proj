
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Models;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class AllDevicesViewModel : ObservableObject
{
    private DeviceItem _deviceItem;

    public AllDevicesViewModel(DeviceItem deviceItem)
    {
        _deviceItem = deviceItem;
        Location = deviceItem.Location ?? "";
        IsActive = deviceItem.IsActive;
        Icon = SetDeviceIcon();
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

    [RelayCommand]
    async Task GoBack() => await Shell.Current.GoToAsync("..");


    private string SetDeviceIcon()
    {
        return DeviceType.ToLower() switch
        {
            "light" => "\uf0eb",
            _ => "\uf2db",
        };
    }
}
