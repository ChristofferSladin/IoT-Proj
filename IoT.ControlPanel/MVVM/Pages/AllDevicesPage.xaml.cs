using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel.MVVM.Pages;

public partial class AllDevicesPage : ContentPage
{
	public AllDevicesPage(AllDevicesViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}