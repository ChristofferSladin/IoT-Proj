﻿using Camera.MAUI;
using CommunityToolkit.Maui;
using IoT.ControlPanel.MVVM.Pages;
using IoT.ControlPanel.MVVM.ViewModels;
using IoT.ControlPanel.MVVM.Views;
using IoT.ControlPanel.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLibrary;
using SharedLibrary.Contexts;
using SharedLibrary.Models.Devices;

namespace IoT.ControlPanel
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCameraView()
                .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesome-Brands");
                    fonts.AddFont("fa-duotone-900.ttf", "FontAwesome-DuoTone");
                    fonts.AddFont("fa-light-300.ttf", "FontAwesome-Light");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesome-Regular");
                    fonts.AddFont("fa-sharp-light-300.ttf", "FontAwesome-Sharp-Light");
                    fonts.AddFont("fa-sharp-regular-400.ttf", "FontAwesome-Sharp-Regular");
                    fonts.AddFont("fa-sharp-solid-900.ttf", "FontAwesome-Sharp-Solid");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesome-Solid");
                    fonts.AddFont("fa-thin-100.ttf", "FontAwesome-Thin");
                });

            builder.Services.AddDbContext<ChristoDbContext>(x => x.UseSqlite($"Data Source={DatabasePathFinder.GetPath()}", x => x.MigrationsAssembly(nameof(SharedLibrary))));

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();

            builder.Services.AddSingleton<AllDevicesViewModel>();
            builder.Services.AddSingleton<AllDevicesPage>();

            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<HomePage>();

            builder.Services.AddSingleton<GetStartedViewModel>();
            builder.Services.AddSingleton<GetStartedPage>();

            builder.Services.AddSingleton<RegisterDeviceViewModel>();
            builder.Services.AddSingleton<RegisterDevicePage>();

            builder.Services.AddSingleton<DeviceManager>();

            builder.Services.AddSingleton<IotHubManager>();

            builder.Services.AddSingleton<WeatherViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}