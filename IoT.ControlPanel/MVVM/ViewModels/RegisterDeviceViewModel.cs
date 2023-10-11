using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IoT.ControlPanel.Services;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Views;
using System.Diagnostics;
using MvvmHelpers.Commands;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class RegisterDeviceViewModel : ObservableObject
{
    private string _deviceId;
    public string DeviceId
    {
        get => _deviceId;
        set
        {
            _deviceId = value;
            OnPropertyChanged();
        }
    }

    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    private Color _messageColor;
    public Color MessageColor
    {
        get => _messageColor;
        set => SetProperty(ref _messageColor, value);
    }

    public ICommand RegisterDeviceCommand { get; }

    private readonly IotHubManager _iotHubManager;

    public RegisterDeviceViewModel(IotHubManager iotHubManager)
    {
        _iotHubManager = iotHubManager;
        RegisterDeviceCommand = new AsyncCommand(RegisterDeviceAsync);
    }

    private async Task RegisterDeviceAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_deviceId))
            {
                Message = "Must enter a Device ID";
                MessageColor = Color.FromArgb("#e94d59");
                return;
            }

            var device = await _iotHubManager.RegisterDeviceAsync(_deviceId);
            if (device != null)
            {
                var connectionString = _iotHubManager.GenerateConnectionString(device);
                Message = $"Device added to IoT Hub.\n\nDevice Id: {device.Id}";
                MessageColor = Color.FromArgb("#41c4a3");
            }
            else
            {
                Message = "Device ID taken";
                MessageColor = Color.FromArgb("#e94d59");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            Message = "An error occurred while registering the device";
        }
    }

    [RelayCommand]
    async Task GoBackToHome()
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }
}
