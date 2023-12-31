﻿using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibrary.Models.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public class DeviceManager
    {
        private readonly DeviceConfiguration _config;
        private readonly DeviceClient _client;

        public DeviceManager(DeviceConfiguration config)
        {
            _config = config;
            _client = DeviceClient.CreateFromConnectionString(_config.ConnectionString);

            Task.FromResult(StartAsync());
        }

        public bool AllowSending() => _config.AllowSending;

        private async Task StartAsync()
        {
            var _telemetryInterval = await  DeviceTwinManager.GetDesiredTwinPropertyAsync(_client, "telemetryInterval");
            await _client.SetMethodDefaultHandlerAsync(DirectMethodDefaultCallback, null);

            if (_telemetryInterval != null)
            {
                _config.TelemetryInterval = int.Parse(_telemetryInterval.ToString()!);
            }
        }

        private async Task<MethodResponse> DirectMethodDefaultCallback(MethodRequest req, object userContext)
        {
            var res = new DirectMethodDataResponse
            {
                Message = $"Executed Method {req.Name} successfully.",
            };

            bool updateTwin = false;

            switch (req.Name.ToLower())
            {
                case "start":
                    {
                        _config.AllowSending = true;
                        updateTwin = true;
                        break;
                    }

                case "stop":
                    {
                        _config.AllowSending = false;
                        updateTwin = true;
                        break;
                    }

                default:
                    {
                        res.Message = $"Direct method {req.Name} not found.";
                        return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)), 404);
                    }
            }

            if (updateTwin)
            {
                await DeviceTwinManager.UpdateReportedTwinPropertyAsync(_client, "AllowSending", _config.AllowSending);
            }

            return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)), 200);
        }
    }
}
