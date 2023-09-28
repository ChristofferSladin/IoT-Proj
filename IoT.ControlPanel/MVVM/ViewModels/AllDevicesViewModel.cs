
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IoT.ControlPanel.MVVM.ViewModels;

public partial class AllDevicesViewModel : ObservableObject
{
    [RelayCommand]
    async Task GoBack() => await Shell.Current.GoToAsync("..");
}
