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
    private void LampSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        _homeViewModel.ToggleLampState(e);

    }
    private void FanSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        _homeViewModel.ToggleFanState(e);
    }
    private void LockSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        _homeViewModel.ToggleLockState(e);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine(_homeViewModel == null ? "HomeViewModel is null" : "HomeViewModel is not null");
        _homeViewModel.UpdateShowconfigMsg();
    }
}