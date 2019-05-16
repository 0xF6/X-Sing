namespace XSign.Worker.Core.SystemTools
{
    using System.Threading.Tasks;

    public interface DNSTools
    {
        Task addRecord(string dnsIP);
        Task resetToDefault();
        Task removeRecord();
    }
}