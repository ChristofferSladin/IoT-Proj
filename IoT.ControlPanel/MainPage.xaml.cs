using IoT.ControlPanel.MVVM.ViewModels;

namespace IoT.ControlPanel
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            // Access the ViewModel and call its method
            var viewModel = (MainViewModel)BindingContext;
            viewModel.ToggleState(e);
        }
    }
}