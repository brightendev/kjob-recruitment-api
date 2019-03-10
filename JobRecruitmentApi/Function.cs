using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JobRecruitmentApi
{
    public static class Function
    {

        [FunctionName("Register")]
        public static async Task<string> RegisterAccount(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)

        {
            log.LogInformation("RegisterAccount Function has been called");

            string requestAccountData = req.Query["account"];

            string email = req.Query["email"];
            string password = req.Query["password"];

            return await Api.Account.CreateAccount(email, password);
        }
    }
}
