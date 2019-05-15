namespace XSign.Worker
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Services;
    using XSing.Core.db;
    using XSing.Core.env;
    using XSing.Core.etc;

    public class Program
    {
        static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder(args)
                .ConfigureJson(x =>
                {
                    x.ContractResolver = new SignalRContractResolver();
                })
                .ConfigureServices(x =>
                {
                    x.AddTransient<SingContext>();
                    x.AddSingleton<WorkerState>();
                    x.AddHostedService<SignalRService>();
                })
                .Build().RunAsync();
        }
    }
}
