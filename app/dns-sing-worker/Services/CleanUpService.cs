namespace XSign.Worker.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using XSing.Core.etc;

    public class CleanUpService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            "error.dump.log".AsFile().When(x => x.Exists, x => x.Delete());

            return Task.CompletedTask;
        }
    }

    public static class Sd
    {
        public static T When<T>(this T t, Func<T, bool> condition, Action<T> actor)
        {
            if (condition(t))
                actor(t);
            return t;
        }
    }

}