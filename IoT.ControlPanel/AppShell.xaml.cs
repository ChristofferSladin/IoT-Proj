using IoT.ControlPanel.MVVM.Pages;
using IoT.ControlPanel.MVVM.Views;

namespace IoT.ControlPanel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(AllDevicesPage), typeof(AllDevicesPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(GetStartedPage), typeof(GetStartedPage));
        }
    }
}