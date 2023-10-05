﻿using CommunityToolkit.Mvvm.ComponentModel;
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

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly DeviceManager _deviceManager;


    public HomeViewModel(DeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
        //IsConfigured = false;
        //IsConfigured = true;

        //if (!IsConfigured)
        //{
        //    IsConfigured = true;
        //}

        Devices = new ObservableCollection<AllDevicesViewModel>(_deviceManager.Devices.Select(device => new AllDevicesViewModel(device)).ToList());

        _deviceManager.DevicesUpdated += UpdateDeviceList;
        Task.FromResult(Initialize());
    }

    public async Task Initialize()
    {
        WeatherViewModel = new WeatherViewModel();
        await WeatherViewModel.GetWeatherAsync();
    }


    [ObservableProperty]
    public WeatherViewModel weatherViewModel;

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
            string methodName = selectedDevice.IsActive ? "Stop" : "Start";
            await AzureIoTHubService.SendDirectMethodAsync(selectedDevice.DeviceId, methodName);

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
    async Task BackToHome() => await Shell.Current.GoToAsync(nameof(HomePage));

    [RelayCommand]
    async Task BackToMain() => await Shell.Current.GoToAsync("//" + nameof(MainPage));
}
