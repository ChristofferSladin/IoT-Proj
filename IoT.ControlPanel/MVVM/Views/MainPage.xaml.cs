using IoT.ControlPanel.MVVM.ViewModels;
using IoT.ControlPanel.Services;

namespace IoT.ControlPanel
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}