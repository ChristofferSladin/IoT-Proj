using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel.MVVM.Views;

public partial class HomePage : ContentPage
{
    private HomeViewModel _homeViewModel;
	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _homeViewModel = viewModel;
	}

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        var viewModel = (HomeViewModel)BindingContext;
        viewModel.ToggleState(e);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine(_homeViewModel == null ? "HomeViewModel is null" : "HomeViewModel is not null");
        _homeViewModel.UpdateShowconfigMsg();
    }
}