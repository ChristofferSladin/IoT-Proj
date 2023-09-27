
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Pages;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    async Task GotoSettings()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}
