using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PetFeeder.Device
{
    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;
        private readonly NetworkManager _networkManager;
        public MainWindow(DeviceManager deviceManager, NetworkManager networkManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _networkManager = networkManager;

            Task.FromResult(CheckConnectivityAsync);
        }

        private async Task CheckConnectivityAsync()
        {
            while (true)
            {
                ConnStatus.Content = await _networkManager.CheckConnectivityAsync();
                await Task.Delay(1000);
            }
        }

        public virtual async void FeedPet_Click(object sender, RoutedEventArgs e)
        {
            // Feed pet logic
            statusLabel.Content = "Fed the pet.";
            await Task.Delay(3000);
            statusLabel.Content = "";


        }
    }
}
