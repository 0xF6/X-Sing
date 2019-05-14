namespace Backend.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using DNS.Client.RequestResolver;
    using Microsoft.Extensions.Hosting;
    using XSing.Core.db;

    public class DNSService : BackgroundService
    {
        private readonly SingContext ctx;
        private readonly IRequestResolver resolver;

        public DNSService(SingContext context, IRequestResolver rev)
        {
            ctx = context;
            resolver = rev;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}