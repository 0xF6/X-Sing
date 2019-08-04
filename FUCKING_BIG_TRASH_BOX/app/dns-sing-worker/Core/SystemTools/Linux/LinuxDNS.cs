namespace XSign.Worker.Core.SystemTools.Linux
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using XSing.Core.etc;

    public class LinuxDNS : DNSTools
    {
        public FileInfo ResolvConfig;
        public LinuxDNS()
        {
            ResolvConfig = "/etc/resolv.conf".AsFile();
        }


        public Task addRecord(string dnsIP)
        {
        }

        public Task resetToDefault()
        {
        }

        public Task removeRecord()
        {
        }
    }
}