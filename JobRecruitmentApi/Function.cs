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
        // ============= Account ======================
        [FunctionName("Register")]
        public static async Task<string> RegisterAccount(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)

        {
            string email = req.Query["email"];
            string password = req.Query["password"];

            log.LogInformation($"RegisterAccount Function has been called with [email: {email}] [password: {password}]");

            return await Api.Account.CreateAccount(email, password);
        }

        [FunctionName("Login")]
        public static async Task<string> Login(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)

        {
            log.LogInformation("Login Function has been called");

            string email = req.Query["email"];
            string password = req.Query["password"];

            return await Api.Account.AuthenticateAsUser(email, password);
        }

        // ============= Database =======================
        [FunctionName("CheckEmail")]
        public static async Task<string> ShowAccount(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string email = req.Query["email"];
            return await Api.Database.CheckEmail(email);
        }
    }
}
