namespace XSing.Core.models.network
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using env;
    using Microsoft.DotNet.PlatformAbstractions;
    using Newtonsoft.Json;
    using RuntimeEnvironment = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment;

    public class WorkerHeader
    {
        public Guid InstanceUID { get; set; }
        public string Version { get; set; }
        public string RuntimeVersion { get; set; }
        public OSInformation OSInfo { get; set; }

        public static string GetRuntimeVersion() => Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<TargetFrameworkAttribute>()?
            .FrameworkName;
        public static WorkerHeader Collect(Guid instanceUID)
        {
            return new WorkerHeader
            {
                InstanceUID = instanceUID,
                OSInfo = OSInformation.Create(),
                RuntimeVersion = GetRuntimeVersion(),
                Version = Env.Version
            };
        }

        public static implicit operator WorkerHeader(string str)
        {
            return JsonConvert.DeserializeObject<WorkerHeader>(str);
        }
        public static implicit operator string(WorkerHeader header)
        {
            return JsonConvert.SerializeObject(header);
        }

        public class OSInformation
        {
            public string OperatingSystem { get; set; }
            public string OperatingSystemVersion { get; set; }
            public string RuntimeArchitecture { get; set; }
            public Platform OperatingSystemPlatform { get; set; }


            public static OSInformation Create()
            {
                return new OSInformation
                {
                    OperatingSystem = RuntimeEnvironment.OperatingSystem,
                    OperatingSystemPlatform = RuntimeEnvironment.OperatingSystemPlatform,
                    OperatingSystemVersion = RuntimeEnvironment.OperatingSystemVersion,
                    RuntimeArchitecture = RuntimeEnvironment.RuntimeArchitecture
                };
            }
        }
    }
}