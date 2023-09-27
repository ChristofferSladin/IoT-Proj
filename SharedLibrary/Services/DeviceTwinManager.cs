
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace SharedLibrary.Services;

public class DeviceTwinManager
{
    public static async Task UpdateReportedTwinPropertyAsync(DeviceClient deviceClient, string key, object value)
    {
        try
        {
            var twinProperties = new TwinCollection();
            twinProperties[key] = value;
            await deviceClient.UpdateReportedPropertiesAsync(twinProperties);
        }
        catch (Exception ex) { Console.WriteLine($"{ex.Message}"); }
    }

    public static async Task<object> GetDesiredTwinPropertyAsync(DeviceClient deviceClient, string key)
    {
        try
        {
            var twin = await deviceClient.GetTwinAsync();
            if (twin.Properties.Desired.Contains(key))
            {
                return twin.Properties.Desired[key];
            }
        }
        catch (Exception ex) { Console.WriteLine($"{ex.Message}"); }
        return null!;
    }
}
