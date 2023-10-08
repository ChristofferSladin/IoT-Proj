using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel.MVVM.Pages;

public partial class AllDevicesPage : ContentPage
{
	private HomeViewModel _homeViewModel;
	public AllDevicesPage(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}