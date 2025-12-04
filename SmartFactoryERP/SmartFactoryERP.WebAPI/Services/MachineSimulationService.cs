using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SmartFactoryERP.Domain.Entities.IoT_Integration;
using SmartFactoryERP.WebAPI.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace SmartFactoryERP.WebAPI.Services
{
    public class MachineSimulationService : BackgroundService
    {
        private readonly IHubContext<MachineHub> _hubContext;
        private readonly Random _random = new Random();
        private readonly List<MachineData> _machines;
        public MachineSimulationService(IHubContext<MachineHub> hubContext)
        {
            _hubContext = hubContext;
            // Initialize 5 simulated machines
            _machines = new List<MachineData>();
            for (int i = 1; i <= 5; i++)
            {
                _machines.Add(new MachineData
                {
                    MachineID = i,
                    Status = MachineDataStatus.Running,
                    Speed = 100,
                    Temperature = 60,
                    ProductionCount = 0,
                    Timestamp = DateTime.Now
                });
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var machine in _machines)
                {
                    UpdateMachineState(machine);
                    CheckForAlerts(machine);
                }
                // Broadcast updates to all connected clients
                await _hubContext.Clients.All.SendAsync("ReceiveMachineData", _machines, stoppingToken);
                // Wait for 2 seconds before next update
                await Task.Delay(2000, stoppingToken);
            }
        }
        private void UpdateMachineState(MachineData machine)
        {
            machine.Timestamp = DateTime.Now;
            // Simulate random status changes (rarely)
            if (_random.NextDouble() > 0.95)
            {
                var statuses = Enum.GetValues(typeof(MachineDataStatus));
                machine.Status = (MachineDataStatus)statuses.GetValue(_random.Next(statuses.Length));
            }
            if (machine.Status == MachineDataStatus.Running)
            {
                // Simulate Speed (fluctuate between 80 and 120)
                machine.Speed = 80 + _random.Next(40);
                // Simulate Temperature (fluctuate between 50 and 100)
                machine.Temperature += _random.Next(-2, 3);
                if (machine.Temperature < 40) machine.Temperature = 40;
                // Simulate Production
                machine.ProductionCount += _random.Next(1, 5);
            }
            else
            {
                machine.Speed = 0;
                // Temperature cools down if stopped
                if (machine.Temperature > 25) machine.Temperature -= 1;
            }
        }
        private async void CheckForAlerts(MachineData machine)
        {
            // Alert if Temperature exceeds 90
            if (machine.Temperature > 90)
            {
                var alert = new MachineAlert
                {
                    MachineID = machine.MachineID,
                    AlertType = MachineAlertType.Critical,
                    AlertMessage = $"High Temperature Detected: {machine.Temperature}°C",
                    AlertTime = DateTime.Now,
                    IsResolved = false
                };
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", alert);
            }
        }
    }
}