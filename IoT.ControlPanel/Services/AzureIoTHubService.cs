using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.ControlPanel.Services
{
    using Microsoft.Azure.Devices;
    using System.Threading.Tasks;

    public class AzureIoTHubService
    {
        private static ServiceClient serviceClient;
        private static string connectionString = "HostName=CS-IoT-Heater-Cooler.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=4TgjS0sBfHZ3P2mEJlA2Us74shGjEh9/0AIoTFcZewQ=";

        public static async Task SendDirectMethodAsync(string deviceId, string methodName)
        {
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            var methodInvocation = new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };

            // If you have any payload to send, set it here
            // methodInvocation.SetPayloadJson("{ \"someValue\": 42 }");

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);

            if (response.Status == 200)
            {
                Console.WriteLine("Direct method invoked successfully");
            }
            else
            {
                Console.WriteLine($"Direct method failed with status: {response.Status}");
            }
        }
    }

}
