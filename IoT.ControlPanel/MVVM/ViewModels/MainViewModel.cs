using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Pages;
using IoT.ControlPanel.Services;
using Microsoft.Azure.Devices;
using SharedLibrary;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DeviceManager _deviceManager;
                                                    //private readonly DataContext _dataContext;
    public MainViewModel(DeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
        //_context = context;

        IsConfigured = false;

                                                    //		var result = Task.FromResult(GetConnectionStringAsync()).Result;
                                                    //		var connectionstring = result.Result;
                                                    //		if (connectionstring != null)
        IsConfigured = true;

        if (!IsConfigured)
        {
                                                    //Task.Run(() => Shell.Current.GoToAsync(nameof(GetStartedPage)));
                                                    //			Task.Run(() => AddConnectionStringAsync("HostName=kyh-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=M/vLVpxoLM7Blwqdsc8YxXaW2A7rQRLjzAIoTFa78jI="));

            IsConfigured = true;
        }

        Devices = new ObservableCollection<AllDevicesViewModel>(_deviceManager.Devices.Select(device => new AllDevicesViewModel(device)).ToList());

        _deviceManager.DevicesUpdated += UpdateDeviceList;
    }

    [ObservableProperty]
    bool isConfigured;

    [ObservableProperty]
    ObservableCollection<AllDevicesViewModel> devices;

    public ICommand ToggleStateCommand { get; private set; }
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

    private void UpdateDeviceList()
    {
        Devices = new ObservableCollection<AllDevicesViewModel>(_deviceManager.Devices.Select(device => new AllDevicesViewModel(device)).ToList());
    }

    public ICommand SendDirectMethodCommand => new RelayCommand<AllDevicesViewModel>(SendDirectMethod);

    private async void SendDirectMethod(AllDevicesViewModel selectedDevice)
    {
        try
        {
            // Determine which direct method to call based on the device's current state
            string methodName = selectedDevice.IsActive ? "Stop" : "Start";

            // Call the direct method asynchronously
            await _deviceManager.SendDirectMethodAsync(selectedDevice.DeviceId, methodName);

            // Update the device's state if the call was successful
            selectedDevice.IsActive = !selectedDevice.IsActive;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

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

    [RelayCommand]
    async static Task GoBack() => await Shell.Current.GoToAsync("..");



    private async Task AddConnectionStringAsync(string connectionString)
    {
        //_context.Settings.Add(new DataAccess.Entities.SettingsEntity { ConnectionString = connectionString });
        //await _context.SaveChangesAsync();
    }

    //private async Task<string> GetConnectionStringAsync()
    //{
    //    //var result = await _context.Settings.FirstOrDefaultAsync();
    //    //if (result != null)
    //    //    return result.ConnectionString;

    //    return null!;
    //}
}
