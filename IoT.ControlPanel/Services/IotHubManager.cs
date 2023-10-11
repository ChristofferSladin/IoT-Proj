
using IoT.ControlPanel.MVVM.ViewModels;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Rest;
using SharedLibrary.Contexts;
using System.Diagnostics;
using Device = Microsoft.Azure.Devices.Device;

namespace IoT.ControlPanel.Services;

public class IotHubManager
{
    private readonly string _connectionString;
    private ChristoDbContext _context;
    private RegistryManager _registryManager;
    private ServiceClient _client;

    public bool IsConfigured;

    public IotHubManager(ChristoDbContext context)
    {
        _context = context;
    }

    public IotHubManager(string connectionString)
    {
        _connectionString = connectionString;
        _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
        _client = ServiceClient.CreateFromConnectionString(connectionString);
    }

    public IotHubManager()
    {
    }

    public async Task<bool> DeleteDeviceAsync(string deviceId)
    {
        try
        {
            await _registryManager.RemoveDeviceAsync(deviceId);
            return true;
        }
        catch (DeviceNotFoundException)
        {
            Debug.WriteLine($"Device with ID {deviceId} not found.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
        }
        return false;
    }

    public async Task<Device> GetDeviceAsync(string deviceId)
    {
        try
        {
            var device = await _registryManager.GetDeviceAsync(deviceId);
            if (device != null)
                return device;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }

    public async Task<Device> RegisterDeviceAsync(string deviceId)
    {
        try
        {
            var device = await _registryManager.AddDeviceAsync(new Device(deviceId));
            if (device != null)
                return device;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }

    public string GenerateConnectionString(Device device)
    {
        try
        {
            return $"{_connectionString.Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }


    public async Task InitializeAsync()
    {
        try
        {
            var settings = await _context.Settings.FirstOrDefaultAsync();
            if (settings != null)
            {
                _registryManager = RegistryManager.CreateFromConnectionString(settings.ConnectionString);
                _client = ServiceClient.CreateFromConnectionString(settings.ConnectionString);

                IsConfigured = true;
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
        catch (Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
    }
}
