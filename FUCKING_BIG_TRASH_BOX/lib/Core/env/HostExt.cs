namespace XSing.Core.env
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public static class HostExt
    {
        public static IHostBuilder ConfigureJson(this IHostBuilder hostBuilder, Action<JsonSerializerSettings> actor = null)
        {
            var setting = new JsonSerializerSettings();
            setting.Converters.Add(new StringEnumConverter());
            actor?.Invoke(setting);
            JsonConvert.DefaultSettings = () => setting;
            return hostBuilder;
        }
        public static IWebHostBuilder ConfigureJson(this IWebHostBuilder hostBuilder, Action<JsonSerializerSettings> actor = null)
        {
            var setting = new JsonSerializerSettings();
            setting.Converters.Add(new StringEnumConverter());
            actor?.Invoke(setting);
            JsonConvert.DefaultSettings = () => setting;
            return hostBuilder;
        }
    }
}