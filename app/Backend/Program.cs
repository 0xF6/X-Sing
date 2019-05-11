namespace Backend
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using DNS.Client.RequestResolver;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;
    using XSing.Core.db;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            // bug netcore 3.0 preview (working directory is the source dir, not output dir)
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe) ?? Path.GetFullPath("./");
            Directory.SetCurrentDirectory(pathToContentRoot);
            // --
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<SingContext>();
                    services.AddTransient<IRequestResolver, DNSResolver>();
                    services.AddHostedService<DNSService>();
                })
                .Build()
                .RunAsync();
        }
    }
}