using System;
using Microsoft.Azure.Devices.Client;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=CS-IoT-Heater-Cooler.azure-devices.net;DeviceId=Console.Device;SharedAccessKey=Q7u3zUuNT3b+waw0cFWZOZTLOWNunqapzHTvpaHQRYs=", TransportType.Mqtt);

    static async Task Main(string[] args)
    {
        // Register direct methods
        await deviceClient.SetMethodHandlerAsync("StartMethod", StartMethod, null);
        await deviceClient.SetMethodHandlerAsync("StopMethod", StopMethod, null);
    }

    static async Task<MethodResponse> StartMethod(MethodRequest methodRequest, object userContext)
    {
        Console.WriteLine("Start method triggered.");
        // Send message to Azure IoT Hub
        var message = new Message(Encoding.ASCII.GetBytes("started"));
        await deviceClient.SendEventAsync(message);
        return new MethodResponse(Encoding.ASCII.GetBytes("{\"status\":\"started\"}"), 200);
    }

    static async Task<MethodResponse> StopMethod(MethodRequest methodRequest, object userContext)
    {
        Console.WriteLine("Stop method triggered.");
        // Send message to Azure IoT Hub
        var message = new Message(Encoding.ASCII.GetBytes("stopped"));
        await deviceClient.SendEventAsync(message);
        return new MethodResponse(Encoding.ASCII.GetBytes("{\"status\":\"stopped\"}"), 200);
    }
}
