using IoT.ControlPanel.MVVM.ViewModels;
using IoT.ControlPanel.Services;

namespace IoT.ControlPanel.MVVM.Views;

public partial class RegisterDevicePage : ContentPage
{
    private RegisterDeviceViewModel ViewModel => (RegisterDeviceViewModel)BindingContext;

    public RegisterDevicePage(IotHubManager iotHubManager)
    {
        InitializeComponent();
        BindingContext = new RegisterDeviceViewModel(iotHubManager);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.Message = string.Empty;
        ViewModel.MessageColor = Color.FromRgba("#FFFFFF");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel.Message = string.Empty;
        ViewModel.MessageColor = Color.FromRgba("#FFFFFF");
    }
}