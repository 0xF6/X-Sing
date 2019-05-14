namespace XSign.Worker.Core
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using XSing.Core.db;
    using XSing.Core.env;
    using XSing.Core.etc;

    /// <summary>
    /// Main State of this DNS worker
    /// </summary>
    public class WorkerState
    {
        private readonly IServiceProvider _provider;

        public Guid InstanceUID { get; }

        public WorkerState(IServiceProvider provider)
        {
            _provider = provider;
            InstanceUID = LockInstanceUID();
            provider.GetService<SingContext>().Database.EnsureCreated();
        }


        public void DumpError(string msg, bool isKill = false)
        {
            lock (Console.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(msg);
                Console.ForegroundColor = ConsoleColor.White;
                File.WriteAllText("error.dump.log", msg);
                if(isKill) Environment.Exit(-0x29A);
            }
        }


        public T GetService<T>() => _provider.GetService<T>();


        public bool IsSupported()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return true;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "/etc/resolv.conf".AsFile().Exists;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return IsServiceAccess();
            return false;
        }

        public bool IsServiceAccess() => true;

        #region Private

        private Guid LockInstanceUID()
        {
            var file = Env.ContentPath.WithCombine("_.uid").AsFile();
            if (file.Exists)
            {
                using var content = file.OpenReader();
                return Guid.Parse(content.ReadLine());
            }

            var uid = Guid.NewGuid();
            file.CreateText().PushLine(uid).Dispose();
            return uid;
        }

        #endregion

        public void Restart()
        {
            // TODO linux
            Process.Start(Process.GetCurrentProcess().StartInfo);
            Process.GetCurrentProcess().Kill();
        }

        public void SwitchDNS(string p0)
        {
            Console.WriteLine($"Set '{p0}' as primary DNS in current system.");
        }

        public void SwitchDNS(bool p0)
        {
            Console.WriteLine($"Reset primary DNS in current system.");
        }
    }
}