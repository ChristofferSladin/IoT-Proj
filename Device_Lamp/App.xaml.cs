using Device_Lamp.Models.Devices;
using Device_Lamp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Device_Lamp;

public partial class App : Application
{
    public static IHost? host { get; set; }

    public App()
    {
        host = Host.CreateDefaultBuilder().ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        })
        .ConfigureServices((config, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton(new DeviceConfiguration(config.Configuration.GetConnectionString("Device")!));
            services.AddSingleton<DeviceManager>();
            services.AddSingleton<NetworkManager>();
        }).Build();

    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await host!.StartAsync();

        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}
