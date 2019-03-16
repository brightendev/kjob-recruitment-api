using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Task = Microsoft.Build.Utilities.Task;

namespace JobRecruitmentApi.Api
{
    static class Account
    {

        public static async Task<string> CreateAccount(string email, string password) {

         //   string graphAccessToken = await System.AzureActiveDirectory.getAccessTokenForGraph();

            string createAccountResponsePayload = await AzureResources.ActiveDirectory.CreateAccount(convertEmailToUsername(email), password);

            string accountId = extractAccountId(createAccountResponsePayload);

            Console.WriteLine($"EMAIL= {email} UID= {accountId}");

            await Api.Database.CreatNewAccount(accountId, email);

            return createAccountResponsePayload;
        }

        public static async Task<string> AuthenticateAsUser(string email, string password) {

            string token = await AzureResources.ActiveDirectory.GetUserAccessToken(convertEmailToUsername(email), password);
            Console.WriteLine($"user token : {token}");
            return await AzureResources.ActiveDirectory.SignInWithToken(token);
        }


        private static string convertEmailToUsername(string email) {

            return email.Replace("@", "_A_").Replace(".", "_D_");
        }

        private static string convertUsernameToEnail(string username) {

            return username.Replace("_A_", "@").Replace("_D_", ".");
        }

        private static string extractAccountId(string payload) {

            JObject respJsonObject = JsonConvert.DeserializeObject(payload) as JObject;

            if(respJsonObject != null) {
                if(respJsonObject["id"] != null) return respJsonObject["id"].Value<string>();

                return payload;
            }

            return "unkown error";
        }
    }
}
