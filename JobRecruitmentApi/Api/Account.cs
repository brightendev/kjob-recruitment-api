using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;

namespace JobRecruitmentApi.Api
{
    static class Account
    {

        public static async Task<string> CreateAccount(string email, string password) {

         //   string graphAccessToken = await System.AzureActiveDirectory.getAccessTokenForGraph();

            return await AzureResources.ActiveDirectory.CreateAccount(convertEmailToUsername(email), password);
        }


        private static string convertEmailToUsername(string email) {

            return email.Replace("@", "_A_").Replace(".", "_D_");
        }

    }
}
