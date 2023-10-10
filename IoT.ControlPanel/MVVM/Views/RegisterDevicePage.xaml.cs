using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel.MVVM.Views;

public partial class RegisterDevicePage : ContentPage
{
	public RegisterDevicePage(RegisterDeviceViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}