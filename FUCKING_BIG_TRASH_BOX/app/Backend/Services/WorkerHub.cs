namespace Backend.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using XSing.Core.models.network;

    public class WorkerHub : Hub
    {
        public static Dictionary<string, WorkerHeader> workers = new Dictionary<string, WorkerHeader>();

        public override Task OnConnectedAsync()
        {
            Term.Success($"Connected new worker");
            return Task.CompletedTask;
            
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var header = workers[Context.ConnectionId];
            Term.Warn($"Disconnect worker at {header.InstanceUID}");
            workers.Remove(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public async Task WAIT(int lo)
        {
            Term.Success($"Start wait {lo} msec...");
            await Task.Delay(lo);
            Term.Success($"Send RESTART...");
            await this.Clients.Caller.SendAsync("ACTION", RequestAction.RESTART);
        }
        public async Task HEADER(WorkerHeader header)
        {
            Term.Success($"Connected new worker: {header.RuntimeVersion}, {header.InstanceUID}, Ver: {header.Version}");
            await Groups.AddToGroupAsync(Context.ConnectionId, header.InstanceUID.ToString());
            await this.Clients.Caller.SendAsync("ACTION", RequestAction.ONLINE);
            workers.Add(Context.ConnectionId, header);
        }
    }
}