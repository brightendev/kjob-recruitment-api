using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentApi.Api
{
    static class Account
    {

        public async static void CreateAccount() {

            string graphAccessToken = await System.AzureActiveDirectory.GetAccessTokenForGraph();

        }

        private static string decodedCreatingAccountRequestData() {

            return "";
        }

    }
}
