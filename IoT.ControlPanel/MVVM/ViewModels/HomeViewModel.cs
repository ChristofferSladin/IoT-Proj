using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Views;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Contexts;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class HomeViewModel :ObservableObject
{
    private readonly ChristoDbContext _context;

    public HomeViewModel(ChristoDbContext dbContext)
    {
        _context = dbContext;
        CheckConfigurationAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    async Task GoToGetStartedPage() => await Shell.Current.GoToAsync(nameof(GetStartedPage)); // BYT NAMN PPÅ COMMAND

    private async Task CheckConfigurationAsync()
    {
        try
        {
            if(await _context.Settings.AnyAsync())
            {
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex);
        }
    }
}
