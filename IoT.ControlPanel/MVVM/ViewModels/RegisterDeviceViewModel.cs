using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IoT.ControlPanel.Services;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class RegisterDeviceViewModel : ObservableObject
{
    //private string _connectionString;
    //public string ConnectionString
    //{
    //    get => _connectionString;
    //    set
    //    {
    //        _connectionString = value;
    //        OnPropertyChanged();
    //    }
    //}

    //public ICommand RegisterDeviceCommand { get; }

    //public RegisterDeviceViewModel()
    //{
    //    RegisterDeviceCommand = new Command(async () => await RegisterDeviceAsync());
    //}

    //private async Task RegisterDeviceAsync()
    //{
    //    var iotHubManager = new IotHubManager(_connectionString);
    //    var device = await iotHubManager.RegisterDeviceAsync("yourDeviceId");
    //}

    //public event PropertyChangedEventHandler PropertyChanged;

    //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}
}
