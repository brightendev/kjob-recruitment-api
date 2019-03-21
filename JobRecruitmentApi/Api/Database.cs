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
            string sqlQuery = $"INSERT INTO [dbo].[Account] ([uid],[email],[role]) VALUES('{uid}','{email}',1);";
            return await AzureResources.SqlDatabase.NoQuery(sqlQuery);
        }

        public static async Task<string> CheckEmail(string email)
        {
            string sqlQuery = $"SELECT * FROM dbo.Account WHERE email='{email}';";
            string st=await AzureResources.SqlDatabase.Query(sqlQuery);
            if (st.Equals("[]"))
            {
                return "OK";
            }
            return "NO";
        }

        public static async Task<string> CreateProfile(string personal_id, string thai_name, string eng_name, string date_of_birth,string nationality, string race, string religion,
            string blood, string relationship, string child, string military_criterion, string address, string province, string telephone, string email, string owner_uid,string gender)
        {
            string sqlQuery = $"INSERT INTO dbo.Profile VALUES('{personal_id}','{thai_name}','{eng_name}','{date_of_birth}','{nationality}','{race}',{religion},{blood}" +
                $",{relationship},{child},{military_criterion},'{address}',{province},'{telephone}','{email}','{owner_uid}',{gender})";
            return await AzureResources.SqlDatabase.NoQuery(sqlQuery);

        }

        public static async Task<string> CheckRole(string uid)
        {
            string sqlQuery = $"SELECT dbo.Role.role_name " +
                $"FROM dbo.Account INNER JOIN dbo.Role ON  dbo.Role.role_id = dbo.Account.role " +
                $"WHERE dbo.Account.uid = '{uid}'; ";
            String result = await AzureResources.SqlDatabase.Query(sqlQuery);
            if (result.Equals("[]")) { return "No Account"; };
            if (result.Equals("ERROR")) { return "ERROR"; }
            result = result.Substring(0, result.Length-3);
            result = result.Substring(15);
            return result;

        }

        public static async Task<string> CheckProfile(string uid)
        {
            string sqlQuery = $"SELECT * FROM dbo.Profile WHERE owner_uid='{uid}'; ";
            string result = await AzureResources.SqlDatabase.Query(sqlQuery);
            if (result.Equals("[]")) { return "NONE"; }
            if (result.Equals("ERROR")) { return "ERROR"; }
            return "HAVE";
        }
        

        public static async Task<string> getPublic()
        {
            string result = "";
            string Religion = await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Religion;");
            string Province= await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Province;");
            string Blood = await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Blood;");
            string Relationship = await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Relationship;");
            string MilitaryCriterion = await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.MilitaryCriterion;");
            result = "{" +
                $"{'"'}Religion{'"'} : {Religion}," +
                $"{'"'}Province{'"'} : {Province}," +
                $"{'"'}Blood{'"'} : {Blood}," +
                $"{'"'}Relationship{'"'} : {Relationship}," +
                $"{'"'}MilitaryCriterion{'"'} : {MilitaryCriterion}" +
                "}";
            return result;
        }
        
    }
}
