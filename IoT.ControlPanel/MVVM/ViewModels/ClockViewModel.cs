using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;


namespace IoT.ControlPanel.MVVM.ViewModels;

public class ClockViewModel : ObservableObject
{

    public ClockViewModel()
    {
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimerElapsed;
        timer.Start();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        var now = DateTime.Now;
        CurrentTime = now.ToString("HH:mm:ss");
        CurrentDate = now.ToString("yyyy-MM-dd");
    }

    private string _currentTime;
    public string CurrentTime
    {
        get => _currentTime;
        set => SetProperty(ref _currentTime, value);
    }

    private string _currentDate;
    public string CurrentDate
    {
        get => _currentDate;
        set => SetProperty(ref _currentDate, value);
    }

    public event PropertyChangedEventHandler propertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
