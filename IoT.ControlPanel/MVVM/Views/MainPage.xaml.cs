using IoT.ControlPanel.MVVM.ViewModels;
using IoT.ControlPanel.Services;

namespace IoT.ControlPanel
{
    public partial class MainPage : ContentPage
    {
        private IotHubManager _iotHubManager;
        public MainPage(MainViewModel viewModel, IotHubManager iotHubManager)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _iotHubManager = iotHubManager;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine(_iotHubManager == null ? "IotHubManager is null" : "IotHubManager is not null");
            _iotHubManager.ResetConfiguration().ConfigureAwait(false);
        }
    }
}