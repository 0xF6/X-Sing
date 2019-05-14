namespace XSign.Worker
{
    using System.Threading.Tasks;
    using Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;
    using XSing.Core.db;

    public class Program
    {
        static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder(args)
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
