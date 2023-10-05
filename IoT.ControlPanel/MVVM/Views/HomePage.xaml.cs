using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel.MVVM.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        // Access the ViewModel and call its method
        var viewModel = (HomeViewModel)BindingContext;
        viewModel.ToggleState(e);
    }
}