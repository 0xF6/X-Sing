namespace XSing.Core.env
{
    using System;
    using System.IO;

    public static class Env
    {
        public static string ContentPath
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                path = Path.Combine(path, $@"xsign\{Version}");
                Directory.CreateDirectory(path);
                return path;
            }
        }
        public static string Version => "0.14";
    }
}