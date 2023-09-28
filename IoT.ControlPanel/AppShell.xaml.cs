using IoT.ControlPanel.MVVM.Pages;

namespace IoT.ControlPanel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(AllDevicesPage), typeof(AllDevicesPage));
        }
    }
}