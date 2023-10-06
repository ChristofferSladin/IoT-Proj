using IoT.ControlPanel.MVVM.ViewModels;
using SharedLibrary.Contexts;
using SharedLibrary.Entities;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ZXing;

namespace IoT.ControlPanel.MVVM.Views;

public partial class GetStartedPage : ContentPage
{

    private readonly ChristoDbContext _context;
	public GetStartedPage(GetStartedViewModel viewModel, ChristoDbContext context)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _context = context;

    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();

            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Delay(500);
                await cameraView.StartCameraAsync();
            });
        }
    }
  
    public void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {

            string pattern = @"HostName=.*;SharedAccessKeyName=.*;SharedAccessKey=.*";
            if(Regex.IsMatch(args.Result[0].Text, pattern))
            {
                try
                {
                    _context.Settings.Add(new AppSettings { ConnectionString = args.Result[0].Text });
                    await _context.SaveChangesAsync();

                   await Shell.Current.GoToAsync(nameof(HomePage));
                }
                catch (Exception ex){ Debug.WriteLine(ex.Message); } 
            }
        });
    }
}