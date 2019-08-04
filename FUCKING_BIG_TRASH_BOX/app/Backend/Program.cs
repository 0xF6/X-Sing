namespace Backend
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using DNS.Client.RequestResolver;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;
    using XSing.Core.db;
    using XSing.Core.env;
    using XSing.Core.etc;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            // bug netcore 3.0 preview (working directory is the source dir, not output dir)
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe) ?? Path.GetFullPath("./");
            Directory.SetCurrentDirectory(pathToContentRoot);
            // --
            await WebHost.CreateDefaultBuilder(args)
                .ConfigureJson()
                .ConfigureServices(services =>
                {
                    services.AddTransient<SingContext>();
                    services.AddTransient<IRequestResolver, DNSResolver>();
                    services.AddHostedService<DNSService>();
                    services.AddMvc(q => q.EnableEndpointRouting = false);
                    services.AddSignalR(x =>
                    {
                        x.EnableDetailedErrors = true;
                    }).AddNewtonsoftJsonProtocol();
                }).Configure(app =>
                {
                    app.UseSignalR(s =>
                    {
                        s.MapHub<WorkerHub>("/signal/hub");
                    });
                    app.UseMvc();
                })
                .UseKestrel(x =>
                {
                    x.AddServerHeader = false;
                    x.ListenLocalhost(6666);
                })
                .Build()
                .RunAsync();
        }
    }
}