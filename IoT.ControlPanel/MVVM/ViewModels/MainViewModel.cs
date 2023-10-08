using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Pages;
using IoT.ControlPanel.MVVM.Views;
using IoT.ControlPanel.Services;
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Contexts;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ChristoDbContext _dbContext;
    private readonly IotHubManager _iotHubManager;
    private Timer _timer;
    private HomeViewModel _homeViewModel;

    public MainViewModel(ChristoDbContext dbContext, IotHubManager iotHubManager)
    {
        _dbContext = dbContext;
        _iotHubManager = iotHubManager;

        CheckConfigurationAsync().ConfigureAwait(true);
    }

    private async Task CheckConfigurationAsync()
    {
        try
        {
            if (await _dbContext.Settings.AnyAsync())
            {
                await _iotHubManager.InitializeAsync();
                await Shell.Current.GoToAsync(nameof(HomePage));   
                
                _iotHubManager.IsConfigured = true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
    }

    [RelayCommand]
    async Task GoToHomePage() 
    {
        await CheckConfigurationAsync();
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    [RelayCommand]
    async Task GoToGetStartedPage() => await Shell.Current.GoToAsync(nameof(GetStartedPage));
}
