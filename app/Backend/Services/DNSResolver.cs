namespace Backend.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using DNS.Client;
    using DNS.Client.RequestResolver;
    using DNS.Protocol;

    public class DNSResolver : IRequestResolver
    {
        public Task<IResponse> Resolve(IRequest request)
        {
            IResponse response = Response.FromRequest(request);

            foreach (var question in response.Questions)
            {
                if (question.Type != RecordType.A)
                {
                    Term.Warn($"Ignored record type: {question.Type}, {question.Name}");
                    //File.AppendAllText("./ignored-records.json", $"{question}\n");
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
}