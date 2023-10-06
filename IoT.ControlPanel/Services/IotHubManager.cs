
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Contexts;
using System.Diagnostics;

namespace IoT.ControlPanel.Services;

public class IotHubManager
{
    private bool isConfigured;
    private ChristoDbContext _context;
    private RegistryManager _registryManager;
    private ServiceClient _client;

    public static bool IsConfigured = false;

    public IotHubManager(ChristoDbContext context)
    {
        _context = context;
    }

    public void InitializeSyncron(object state) // Modify the method signature to match TimerCallback
    {
        Task.Run(() => InitializeAsync());
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (!isConfigured)
            {
                var settings = await _context.Settings.FirstOrDefaultAsync();
                if (settings != null)
                {
                    _registryManager = RegistryManager.CreateFromConnectionString(settings.ConnectionString);
                    _client = ServiceClient.CreateFromConnectionString(settings.ConnectionString);
                    isConfigured = true;
                    IsConfigured = true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }

    public async Task ResetConfiguration()
    {
        try
        {
            var settingsList = await _context.Settings.ToListAsync();

            if (settingsList.Any())   // HERE IT JUMPS OUT OF THE TRY AND INTO THE CATCH,     why?
            {
                _context.Settings.RemoveRange(settingsList);
                await _context.SaveChangesAsync(); 
            }
        }
        catch (Exception ex)
        { Debug.WriteLine(ex.Message); }
    }
}
