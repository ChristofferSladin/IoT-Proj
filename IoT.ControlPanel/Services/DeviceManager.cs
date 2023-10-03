
using IoT.ControlPanel.MVVM.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using System.Diagnostics;
using System.Timers;

namespace IoT.ControlPanel.Services;
public class DeviceManager
{
    private readonly string _connectionString = "HostName=CS-IoT-Heater-Cooler.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=4TgjS0sBfHZ3P2mEJlA2Us74shGjEh9/0AIoTFcZewQ=";
    private readonly RegistryManager _registryManager;
    private readonly ServiceClient _serviceClient;
    private readonly System.Timers.Timer _timer;

    public List<DeviceItem> Devices { get; private set; }
    public event Action DevicesUpdated;

    public DeviceManager()
    {

        _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
        _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);

        Devices = new List<DeviceItem>();

        _timer = new System.Timers.Timer(5000);
        _timer.Elapsed += async (s, e) => await GetAllDevicesAsync();
        _timer.Start();
    }

    public async Task SendDirectMethodAsync(string deviceId, string methodName)
    {
        var methodInvocation = new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };

        // If you have any payload to send, set it here
        // methodInvocation.SetPayloadJson("{ \"someValue\": 42 }");

        var response = await _serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);

        if (response.Status == 200)
        {
            Console.WriteLine("Direct method invoked successfully");
        }
        else
        {
            Console.WriteLine($"Direct method failed with status: {response.Status}");
        }
    }

    private async Task GetAllDevicesAsync()
    {
        try
        {

            // IF sats (isConfigured) hämta listan om appen har en connection string


            var updated = false;
            var list = new List<Twin>();
            var result = _registryManager.CreateQuery("select * from devices");

            foreach (var item in await result.GetNextAsTwinAsync())
                list.Add(item);

            foreach (var device in list)
                if (!Devices.Any(x => x.DeviceId == device.DeviceId))
                {
                    var _device = new DeviceItem { DeviceId = device.DeviceId };

                    try { _device.DeviceType = device.Properties.Reported["deviceType"].ToString(); }
                    catch (Exception ex) { Debug.WriteLine(ex); }

                    try { _device.Vendor = device.Properties.Reported["vendor"].ToString(); }
                    catch (Exception ex) { Debug.WriteLine(ex); }

                    try { _device.Location = device.Properties.Reported["location"].ToString(); }
                    catch (Exception ex) { Debug.WriteLine(ex); }

                    try { _device.IsActive = bool.Parse(!string.IsNullOrEmpty(device.Properties.Reported["isActive"].ToString())); }
                    catch (Exception ex) { Debug.WriteLine(ex); }

                    Devices.Add(_device);
                    updated = true;
                }

            for (int i = Devices.Count - 1; i >= 0; i--)
            {
                if (!list.Any(x => x.DeviceId == Devices[i].DeviceId))
                {
                    Devices.RemoveAt(i);
                    updated = true;
                }
            }


            if (updated)
                DevicesUpdated.Invoke();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
        }
    }
}
