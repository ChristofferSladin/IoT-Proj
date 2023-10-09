using IoT.ControlPanel.MVVM.ViewModels;
using IoT.ControlPanel.Services;

namespace IoT.ControlPanel
{
    public partial class MainPage : ContentPage
    {
        private IotHubManager _iotHubManager;
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}