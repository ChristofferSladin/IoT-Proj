
using IoT.ControlPanel.MVVM.ViewModels;
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Contexts;
using System.Diagnostics;

namespace IoT.ControlPanel.Services;

public class IotHubManager
{
    private ChristoDbContext _context;
    private RegistryManager _registryManager;
    private ServiceClient _client;

    public bool IsConfigured;

    public IotHubManager(ChristoDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()    // TITTA IGENOM DENNA METOD; KAN VARA SÅ ATT ISCONFIURED INTE FUNGERAR SOM DEN SKA, false när den ska va true o tvärtom
    {
        try
        {
            if (!IsConfigured)
            {
                var settings = await _context.Settings.FirstOrDefaultAsync();
                if (settings != null)
                {
                    _registryManager = RegistryManager.CreateFromConnectionString(settings.ConnectionString);
                    _client = ServiceClient.CreateFromConnectionString(settings.ConnectionString);

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
            if (settingsList.Any())
            {
                _context.Settings.RemoveRange(settingsList);
                await _context.SaveChangesAsync();
                IsConfigured = false;

            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }
}
