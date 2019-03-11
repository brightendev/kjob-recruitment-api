using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JobRecruitmentApi.Api
{
    static class Account
    {

        public static async Task<string> CreateAccount(string email, string password) {

         //   string graphAccessToken = await System.AzureActiveDirectory.getAccessTokenForGraph();

            string createAccountResponsePayload = await AzureResources.ActiveDirectory.CreateAccount(convertEmailToUsername(email), password);

            string accountId = extractAccountId(createAccountResponsePayload);

            // TODO call Database API to initialize account data.
            return accountId;
        }


        private static string convertEmailToUsername(string email) {

            return email.Replace("@", "_A_").Replace(".", "_D_");
        }

        private static string extractAccountId(string payload) {

            JObject respJsonObject = JsonConvert.DeserializeObject(payload) as JObject;

            if(respJsonObject != null) {
                if(respJsonObject["id"] != null) return respJsonObject["id"].Value<string>();

                return "creating account failed";
            }

            return "unkown error";
        }
    }
}
