using Microsoft.AspNetCore.SignalR;
using SmartFactoryERP.Domain.Entities.IoT_Integration;
namespace SmartFactoryERP.WebAPI.Hubs
{
    public class MachineHub : Hub
    {
        // Method to broadcast machine data to all connected clients
        public async Task BroadcastMachineData(List<MachineData> data)
        {
            await Clients.All.SendAsync("ReceiveMachineData", data);
        }
        // Method to broadcast alerts
        public async Task BroadcastAlert(MachineAlert alert)
        {
            await Clients.All.SendAsync("ReceiveAlert", alert);
        }
    }
}