using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoT.ControlPanel.MVVM.Views;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [RelayCommand]
    async Task BackToHome() => await Shell.Current.GoToAsync(nameof(HomePage));
}
