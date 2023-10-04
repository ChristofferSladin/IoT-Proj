using CommunityToolkit.Mvvm.ComponentModel;
using IoT.ControlPanel.MVVM.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace IoT.ControlPanel.MVVM.ViewModels
{
    public partial class WeatherViewModel : ObservableObject
    {
        private Timer _timer;

        public WeatherViewModel()
        {
            _timer = new Timer(AsyncCallback, null, 0, 900000);
        }

        private async void AsyncCallback(object state)
        {
            await GetWeatherAsync();
        }

        private string _location;
        private string _temperature;
        private string _weatherDescription;
        private double _windSpeed;
        private int _humidity;
        private string _icon;

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public string Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        public string WeatherDescription
        {
            get => _weatherDescription;
            set => SetProperty(ref _weatherDescription, value);
        }

        public double WindSpeed
        {
            get => _windSpeed;
            set => SetProperty(ref _windSpeed, value);
        }

        public int Humidity
        {
            get => _humidity;
            set => SetProperty(ref _humidity, value);
        }

        double ConvertFahrenheitToCelsius(double fahrenheit)
        {
            double celsius = (fahrenheit - 32) / 1.8;
            return Math.Round(celsius);
        }

        public string GetWeatherIcon(string iconCode)
        {
            switch (iconCode)
            {
                case "01d":
                    return "\uf185";  // fas fa-sun
                case "01n":
                    return "\uf2c9";  // fas fa-moon (assuming moon for clear sky at night)
                case "02d":
                case "02n":
                    return "\uf6c4";  // fas fa-cloud-sun
                case "03d":
                case "03n":
                    return "\uf6c4";  // fas fa-cloud-sun (assuming partly cloudy for scattered clouds)
                case "04d":
                case "04n":
                    return "\uf0c2";  // fas fa-cloud
                case "09d":
                case "09n":
                    return "\uf73d";  // fas fa-cloud-rain
                case "10d":
                case "10n":
                    return "\uf740";  // fas fa-cloud-showers-heavy
                case "11d":
                case "11n":
                    return "\uf0e7";  // fas fa-bolt
                case "13d":
                case "13n":
                    return "\uf2dc";  // fas fa-snowflake
                case "50d":
                case "50n":
                    return "\uf75f";  // fas fa-smog (assuming smog for mist)
                default:
                    return "";  // Return an empty string or a default icon code if iconCode is not recognized
            }
        }


        public async Task GetWeatherAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://open-weather13.p.rapidapi.com/city/stockholm"),
                Headers =
                {
                    { "X-RapidAPI-Key", "2a85b7850dmshd169ee09dede708p1584fcjsn15788fd3d197" },
                    { "X-RapidAPI-Host", "open-weather13.p.rapidapi.com" },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<WeatherObject>(body);

                // Populate individual properties
                Location = data.Name;
                Icon = GetWeatherIcon(data.Weather[0].Icon);
                Temperature = $"{ConvertFahrenheitToCelsius(data.Main.Temp)}°C";
                WeatherDescription = $"{data.Weather[0].Main} ({data.Weather[0].Description})";
                WindSpeed = data.Wind.Speed;
                Humidity = data.Main.Humidity;
            }
        }
    }
}



