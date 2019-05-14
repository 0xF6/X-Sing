namespace XSign.Worker.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Hosting;
    using XSing.Core.db;
    using XSing.Core.db.models;
    using XSing.Core.models.network;

    public class SignalRService : BackgroundService
    {
        private readonly WorkerState _state;
        private SingContext ctx { get; }
        public string Url { get; private set; }

        public HubConnection SigConnection { get; private set; }

        public SignalRService(SingContext context, WorkerState state)
        {
            _state = state;
            ctx = context;
        }

        public Dictionary<string, IDisposable> methodStorage = new Dictionary<string, IDisposable>();


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_state.IsSupported())
            {
                _state.DumpError($"Current OS is not supported.", true);
            }



            Url = $"signal-server".OrDefault("http://localhost:666/signal/hub");
            SigConnection = new HubConnectionBuilder()
                .WithUrl(Url)
                .Build();

            SigConnection.Closed += OnClosed;

            await SigConnection.StartAsync(stoppingToken);
            await SigConnection.SendCoreAsync("HEADER", 
                new object[] { WorkerHeader.Collect(_state.InstanceUID) }, stoppingToken);
        }

        private async Task OnClosed(Exception arg)
        {
            await Task.Delay(1000);
            await SigConnection.StartAsync();
        }


        
    }

}