using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel.MVVM.Pages;

public partial class AllDevicesPage : ContentPage
{
	public AllDevicesPage(MainViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}