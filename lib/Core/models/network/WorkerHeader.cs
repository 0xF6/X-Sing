namespace XSing.Core.models.network
{
    using System;
    using System.Reflection;
    using System.Runtime.Versioning;
    using env;

    public struct WorkerHeader
    {
        public Guid InstanceUID { get; set; }
        public string Version { get; set; }
        public string RuntimeVersion { get; set; }
        public OperatingSystem OSInfo { get; set; }

        public static string GetRuntimeVersion() => Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<TargetFrameworkAttribute>()?
            .FrameworkName;
        public static WorkerHeader Collect(Guid instanceUID)
        {
            return new WorkerHeader
            {
                InstanceUID = instanceUID,
                OSInfo = Environment.OSVersion,
                RuntimeVersion = GetRuntimeVersion(),
                Version = Env.Version
            };
        }
    }
}