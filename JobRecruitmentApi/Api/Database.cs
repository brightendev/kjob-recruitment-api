using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobRecruitmentApi.Api
{
    static class Database
    {
        public static async Task<string> CreatNewAccount(string uid, string email)
        {
            if (uid.Equals("") || uid == null)
            {
                return "ERROR";
            }
            if (email.Equals("") || email == null)
            {
                return "ERROR";
            }
            string sqlQuery = $"INSERT INTO [dbo].[Account] ([uid],[email],[role]) VALUES('{uid}','{email}',1);";
            return await AzureResources.SqlDatabase.Insert(sqlQuery);
        }
        public static async Task<string> ShowAccount()
        {          
            string sqlQuery = $"SELECT * FROM dbo.Account";
            return await AzureResources.SqlDatabase.Select(sqlQuery);
        }



    }
}
