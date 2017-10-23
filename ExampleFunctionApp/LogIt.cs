using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using NLog;

namespace ExampleFunctionApp
{
    public static class LogIt
    {
        [FunctionName("LogIt")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            // call configration for azure functions 
            NLog.Functions.Configuration.Initialize(log);

            var logger = LogManager.GetLogger("LogIt");

            logger.Info("C# HTTP trigger function processed a request.");

            return req.CreateResponse(HttpStatusCode.OK, "Hello NLog");
        }
    }
}
