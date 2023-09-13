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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;
        private readonly NetworkManager _networkManager;
        public MainWindow(DeviceManager deviceManager, NetworkManager networkManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _networkManager = networkManager;

            petTypeComboBox.Items.Add("Cat");
            petTypeComboBox.Items.Add("Dog");
            petTypeComboBox.Items.Add("Lizard");

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

        private void PetTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Update based on pet type
        }

        private void PortionSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update portion size
        }
    }
}
