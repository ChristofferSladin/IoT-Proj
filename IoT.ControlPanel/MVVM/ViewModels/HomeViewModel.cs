using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Views;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Contexts;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using IoT.ControlPanel.Services;
using System.Windows.Input;
using IoT.ControlPanel.MVVM.Pages;
using SharedLibrary.Models.Devices;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly string _deviceConnectionString = "HostName=CS-IoT-Heater-Cooler.azure-devices.net;DeviceId=Device.Fan;SharedAccessKey=pDZBfBVrY14g1u/N9DOcbh9f2NA4OBZj00DqMGIh0rw=";
    private readonly DeviceManager _deviceManager;
    private readonly IotHubManager _iotHubManager;
    private readonly System.Threading.Timer _timer;

    public HomeViewModel(DeviceManager deviceManager, IotHubManager iotHubManager)
    {
        _deviceManager = deviceManager;
        _iotHubManager = iotHubManager;

        Devices = new ObservableCollection<AllDevicesViewModel>(_deviceManager.Devices.Select(device => new AllDevicesViewModel(device, iotHubManager)).ToList());

        _deviceManager.DevicesUpdated += UpdateDeviceList;
        Task.FromResult(Initialize());
    }

    public async Task Initialize()
    {
        ClockViewModel = new ClockViewModel();
        WeatherViewModel = new WeatherViewModel();
        await WeatherViewModel.GetWeatherAsync();
    }

    [ObservableProperty]
    public ClockViewModel clockViewModel;

    [ObservableProperty]
    public WeatherViewModel weatherViewModel;

    [ObservableProperty]
    bool isConfigured;

    [ObservableProperty]
    ObservableCollection<AllDevicesViewModel> devices;

    [ObservableProperty]
    string showConfigMsgReason;

    [ObservableProperty]
    string showConfigMsgSolution;

    public void UpdateShowconfigMsg()
    {
        if (_iotHubManager.IsConfigured)
        {
            ShowConfigMsgReason = string.Empty;
            ShowConfigMsgSolution = string.Empty;
        }
        else if (!_iotHubManager.IsConfigured)
        {
            ShowConfigMsgReason = "Application NOT Configured";
            ShowConfigMsgSolution = "Please go back and scan your IOT Hub QR-Code";
        }
    }

    public ICommand SendDirectMethodCommand => new RelayCommand<AllDevicesViewModel>(SendDirectMethod);

    private async void SendDirectMethod(AllDevicesViewModel selectedDevice)
    {
        try
        {
            string methodName = selectedDevice.IsActive ? "Stop" : "Start";
            await _deviceManager.SendDirectMethodAsync(selectedDevice.DeviceId, methodName);

            selectedDevice.IsActive = !selectedDevice.IsActive;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public ICommand ToggleFanStateCommand { get; private set; }

    public async void ToggleFanState(ToggledEventArgs e)
    {
        bool isToggled = e.Value;
        var deviceId = "Device.Fan";
        string methodName = isToggled ? "start" : "stop";
        try
        {
            await _deviceManager.SendDirectMethodAsync(deviceId, methodName);
            IsFanConnected = isToggled;  
        }
        catch (Microsoft.Azure.Devices.Common.Exceptions.DeviceNotFoundException)
        {
            IsFanConnected = false; 
            FanConnectionStatusText = "Device Not Connected";
            IsFanConnectionStatusVisible = true;

            await Task.Delay(3000);

            IsFanConnectionStatusVisible = false;
        }
    }
    public ICommand ToggleLampStateCommand { get; private set; }

    public async void ToggleLampState(ToggledEventArgs e)
    {
        bool isToggled = e.Value;
        var deviceId = "Device_Lamp";
        string methodName = isToggled ? "start" : "stop";
        try
        {
            await _deviceManager.SendDirectMethodAsync(deviceId, methodName);
            IsLampConnected = isToggled;
        }
        catch (Microsoft.Azure.Devices.Common.Exceptions.DeviceNotFoundException)
        {
            IsLampConnected = false;
            LampConnectionStatusText = "Device Not Connected";
            IsLampConnectionStatusVisible = true;

            await Task.Delay(3000);

            IsLampConnectionStatusVisible = false;
        }
    }

    public ICommand ToggleLockStateCommand { get; private set; }
    public async void ToggleLockState(ToggledEventArgs e)
    {
        bool isToggled = e.Value;
        var deviceId = "Device_SmartLock";
        string methodName = isToggled ? "start" : "stop";
        try
        {
            await _deviceManager.SendDirectMethodAsync(deviceId, methodName);
            IsLockConnected = isToggled;
        }
        catch (Microsoft.Azure.Devices.Common.Exceptions.DeviceNotFoundException)
        {
            IsLockConnected = false;
            LockConnectionStatusText = "Device Not Connected";
            IsLockConnectionStatusVisible = true;

            await Task.Delay(3000);

            IsLockConnectionStatusVisible = false;
        }
    }
    private void UpdateDeviceList()
    {
        Devices = new ObservableCollection<AllDevicesViewModel>(_deviceManager.Devices.Select(device => new AllDevicesViewModel(device, _iotHubManager)).ToList());
    }

    private string _fanConnectionStatusText;
    public string FanConnectionStatusText
    {
        get => _fanConnectionStatusText;
        set => SetProperty(ref _fanConnectionStatusText, value);
    }

    private bool _isFanConnectionStatusVisible;
    public bool IsFanConnectionStatusVisible
    {
        get => _isFanConnectionStatusVisible;
        set => SetProperty(ref _isFanConnectionStatusVisible, value);
    }

    private bool _isFanConnected;
    public bool IsFanConnected
    {
        get => _isFanConnected;
        set => SetProperty(ref _isFanConnected, value);
    }

    // _____________________________________

    private string _lampConnectionStatusText;
    public string LampConnectionStatusText
    {
        get => _lampConnectionStatusText;
        set => SetProperty(ref _lampConnectionStatusText, value);
    }

    private bool _isLampConnectionStatusVisible;
    public bool IsLampConnectionStatusVisible
    {
        get => _isLampConnectionStatusVisible;
        set => SetProperty(ref _isLampConnectionStatusVisible, value);
    }

    private bool _isLampConnected;
    public bool IsLampConnected
    {
        get => _isLampConnected;
        set => SetProperty(ref _isLampConnected, value);
    }

    // _________________________________________________

    private string _lockConnectionStatusText;
    public string LockConnectionStatusText
    {
        get => _lockConnectionStatusText;
        set => SetProperty(ref _lockConnectionStatusText, value);
    }

    private bool _isLockConnectionStatusVisible;
    public bool IsLockConnectionStatusVisible
    {
        get => _isLockConnectionStatusVisible;
        set => SetProperty(ref _isLockConnectionStatusVisible, value);
    }

    private bool _isLockConnected;
    public bool IsLockConnected
    {
        get => _isLockConnected;
        set => SetProperty(ref _isLockConnected, value);
    }


    [RelayCommand]
    async Task GotoSettings() => await Shell.Current.GoToAsync(nameof(SettingsPage));

    [RelayCommand]
    async Task GoToAllDevices() => await Shell.Current.GoToAsync(nameof(AllDevicesPage));

    [RelayCommand]
    async Task BackToHome()
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    [RelayCommand]
    async Task BackToMain()
    {
        await _iotHubManager.ResetConfiguration();
        await Shell.Current.GoToAsync("//" + nameof(MainPage));
    }

    [RelayCommand]
    async Task GoToregisterDevice()
    {
        await Shell.Current.GoToAsync(nameof(RegisterDevicePage));
    }
}
