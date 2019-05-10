namespace Backend
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using DNS.Client;
    using DNS.Client.RequestResolver;
    using DNS.Protocol;
    using DNS.Protocol.ResourceRecords;
    using DNS.Server;
    using XSing.Core.env;

    public static class Program
    {
        public class LocalRequestResolver : IRequestResolver
        {
            public Task<IResponse> Resolve(IRequest request)
            {
                IResponse response = Response.FromRequest(request);

                foreach (var question in response.Questions)
                {
                    if (question.Type != RecordType.A)
                    {
                        Term.Warn($"Ignored record type: {question.Type}, {question.Name}");
                        File.AppendAllText("./ignored-records.json", $"{question}\n");
                        continue;
                    }

                    try
                    {
                        var result = new DnsClient("1.1.1.1").Resolve(question.Name, question.Type).Result
                            .AnswerRecords;
                        foreach (var resultAnswerRecord in result)
                            response.AnswerRecords.Add(resultAnswerRecord);
                    }
                    catch (Exception)
                    {
                        Term.Error($"NAME ERROR => {question}");
                        response.ResponseCode = ResponseCode.NameError;
                    }
                }
                return Task.FromResult(response);
            }
        }
        public static async Task Main(string[] args)
        {
            // bug netcore 3.0 preview (working directory is the source dir, not output dir)
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe) ?? Path.GetFullPath("./");
            Directory.SetCurrentDirectory(pathToContentRoot);
            // --
            Term.Warn($"Content Path: {Env.ContentPath}");
            var masterFile = new MasterFile();
            var server1 = new DnsServer(new LocalRequestResolver());

            
            masterFile.AddIPAddressResourceRecord("google.com", "127.0.0.1");
            masterFile.AddIPAddressResourceRecord("github.com", "127.0.0.1");

            server1.Listening += (sender, eventArgs) =>
            {
                var dns = new DnsClient("127.0.0.1");
                dns.Resolve("ya.ru", RecordType.A).GetAwaiter().GetResult();
            };
            server1.Requested += (sender, e) => Term.Success($"LOOKUP  => {e.Request.Questions.First()}");
            server1.Responded += (sender, e) =>
            {
                if (e.Response.AnswerRecords.Any())
                {
                    var q = e.Response.AnswerRecords.First();
                    Term.Success($"RESOLVE => {q}");
                    File.AppendAllText("./resolved-records.json", $"{q}\n");
                }
                    
            };
            server1.Errored += (sender, e) => Term.Error(e.Exception.Message);


            await server1.Listen();
        }

        public static class Term
        {
            private static readonly object guarder = new object();
            public static void Error(object o)
            {
                lock (guarder)
                {
                    Head("ERROR", ConsoleColor.Red);
                    Console.WriteLine(o);
                }
            }
            public static void Success(object o)
            {
                lock (guarder)
                {
                    Head("SUCCESS", ConsoleColor.Green);
                    Console.WriteLine(o);
                }
            }
            public static void Warn(object o)
            {
                lock (guarder)
                {
                    Head("WARN", ConsoleColor.Yellow);
                    Console.WriteLine(o);
                }
            }
            private static void Head(string headType, ConsoleColor color)
            {
                Console.Write("[");
                Console.ForegroundColor = color;
                Console.Write(headType);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("]: ");
            }
        }
    }
}