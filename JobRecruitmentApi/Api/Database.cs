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
            if ("ERROR".Equals(result.Substring(4))) { return "ERROR"; }
            result = result.Substring(0, result.Length-3);
            result = result.Substring(15);
            return result;
        }

        public static async Task<string> CheckProfile(string uid)
        {
            string sqlQuery = $"SELECT * FROM dbo.Profile WHERE owner_uid = '{uid}' ; ";
            string result = await AzureResources.SqlDatabase.Query(sqlQuery);
            if (result.Equals("[]")) { return "NONE"; }
            if ("ERROR".Equals(result.Substring(4))) { return "ERROR"; }
            return "HAVE";
        }
        public static async Task<string> CheckAccount(string uid)
        {
            string sqlQuery = $"SELECT email FROM dbo.Account WHERE uid = '{uid}'; ";
            string result = await AzureResources.SqlDatabase.Query(sqlQuery);
            if (result.Equals("[]")) { return "NONE"; }
            if ("ERROR".Equals(result.Substring(4))) { return "ERROR"; }
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
            string Gender = await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Gender;");
            result = "{" +
                $"{'"'}Religion{'"'} : {Religion}," +
                $"{'"'}Province{'"'} : {Province}," +
                $"{'"'}Blood{'"'} : {Blood}," +
                $"{'"'}Relationship{'"'} : {Relationship}," +
                $"{'"'}MilitaryCriterion{'"'} : {MilitaryCriterion}," +
                $"{'"'}Gender{'"'} : {Gender}" +
                "}";
            return result;
        }

        public static async Task<string> GetReligion() => await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Religion;");

        public static async Task<string> GetBlood() => await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Blood;");

        public static async Task<string> GetRelationship() => await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Relationship;");

        public static async Task<string> GetProvince() => await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.Province;");

        public static async Task<string> GetMilitaryCriterion() => await AzureResources.SqlDatabase.Query($"SELECT * FROM dbo.MilitaryCriterion;");

        public static async Task<string> GetAccount(string email) {       
            string sqlQuery = $"SELECT * FROM dbo.Account WHERE email='{email}' AND role=3;";
            string cherckRole = await AzureResources.SqlDatabase.Query(sqlQuery);
            if (cherckRole.Equals("[]")) {
                return "Not entitled";
            }
            sqlQuery = "SELECT Account.email , Account.last_login , Role.role_name FROM Account INNER JOIN Role ON  Role.role_id = Account.role; ";
            return await AzureResources.SqlDatabase.Query(sqlQuery);


        }

        public static async Task<string> AccountData(string uid) {
            string sqlQueryAccount = $"SELECT Account.email , Account.last_login , Account.created_date " +
                $"FROM Account WHERE uid='{uid}';";
            string account =
 await AzureResources.SqlDatabase.QueryOne(sqlQueryAccount);
            if (account.Equals("")) {
                return "Not Account";
            }
            string sqlQueryProfil = $"SELECT Profile.personal_id,Profile.thai_name,Profile.eng_name,Profile.date_of_birth,Profile.nationality," +
                $"Profile.race,Religion.religion_name,Blood.blood_name,Relationship.relationship_name,Profile.child," +
                $"MilitaryCriterion.military_criterion_name,Profile.address,Province.province_name,Profile.telephone," +
                $"Profile.email,Gender.gender_name FROM Profile " +
                $" INNER JOIN Religion ON Profile.religion=Religion.religion_id " +
                $" INNER JOIN Blood ON Profile.blood=Blood.blood_id " +
                $" INNER JOIN Relationship ON Profile.relationship=Relationship.relationship_id " +
                $" INNER JOIN MilitaryCriterion ON Profile.military_criterion=MilitaryCriterion.military_criterion_id " +
                $" INNER JOIN Province ON Profile.province=Province.province_id" +
                $" INNER JOIN Gender ON Profile.gender=Gender.gender_id " +
                $" WHERE owner_uid = '{uid}';";
            string profile = await AzureResources.SqlDatabase.QueryOne(sqlQueryProfil);
            if (profile.Equals("")) {
                return "Not Profile";
            }
            string Json = "{" +
                $"{'"'}Account{'"'} : {account}," +
                $"{'"'}Profile{'"'} : {profile}" +
                "}";
            return Json;




        }

        public static async Task<string> EditUserNotif(string edit,string value,string uid)
        {
            string setting = edit.Substring(8, edit.Length - 8);
            string sql;
            string result;
            string checkAccount = await CheckAccount(uid);
            if (! checkAccount.Equals("HAVE"))
            {
                return "{" +
                        $"{'"'}error{'"'} : {'"'}NOT ACCOUNT{'"'} " +
                        "}";
            }
            if (value.Equals("on"))
            {
                sql = $"UPDATE Account SET {setting} = 'True' WHERE uid='{uid}';";
                result = await AzureResources.SqlDatabase.NoQuery(sql);
                if (result.Equals("OK"))
                {
                    return "{" +
                        $"{'"'}result{'"'} : {'"'}success{'"'}," +
                        $"{'"'}{setting}{'"'} : {'"'}on{'"'} " +
                        "}";
                }
                else
                {
                    return "{" +
                        $"{'"'}error{'"'} : {'"'}{result}{'"'} " +
                        "}";
                }
            }
            else
            {
                sql = $"UPDATE Account SET {setting} = 'False' WHERE uid='{uid}';";
                result = await AzureResources.SqlDatabase.NoQuery(sql);
                if (result.Equals("OK"))
                {
                    return "{" +
                        $"{'"'}result{'"'} : {'"'}success{'"'}," +
                        $"{'"'}{setting}{'"'} : {'"'}off{'"'}" +
                        "}";
                }
                else
                {
                    return "{" +
                        $"{'"'}error{'"'} : {'"'}{result}{'"'} " +
                        "}";
                }

            }
           
        }

        public static async Task<string> EditProfile(string edit, string value, string uid)
        {
            string check = await CheckAccount(uid);
            if (!check.Equals("HAVE"))
            {
                return "{" +
                        $"{'"'}error{'"'} : {'"'}NOT ACCOUNT{'"'} " +
                        "}";
            }
            if (check.Equals("[]"))
            {
                return "{" +
                        $"{'"'}error{'"'} : {'"'}NOT Profile{'"'} " +
                        "}";
            }
            check = await CheckProfile(uid);
            string[] st = { "gender", "blood", "relationship", "child", "military_criterion", "province" };
            if (Array.IndexOf(st, edit) > 0)
            {
                return await EditProfile2(edit, value, uid);
            }
            else
            {
                return await EditProfile1(edit, value, uid);
            }
        }

        private static async Task<string> EditProfile1(string edit, string value, string uid)
        {
            string sql = $"UPDATE Profile SET {edit} = '{value}' WHERE owner_uid = '{uid}';";
            string result = await AzureResources.SqlDatabase.NoQuery(sql);
            if (result.Equals("OK"))
            {
                return "{" +
                    $"{'"'}result{'"'} : {'"'}success{'"'}," +
                    $"{'"'}{edit}{'"'} : {'"'}{value}{'"'}" +
                    "}";
            }
            else
            {
                return "{" +
                       $"error : '{result}' " +
                       "}";
            }

        }

        private static async Task<string> EditProfile2(string edit, string value, string uid)
        {
            string sql = $"UPDATE Profile SET {edit} = {value}  WHERE owner_uid = '{uid}';";
            string result = await AzureResources.SqlDatabase.NoQuery(sql);
            if (result.Equals("OK"))
            {
                return "{" +
                    $"{'"'}result{'"'} : {'"'}success{'"'}," +
                    $"{'"'}{edit}{'"'} : {value}" +
                    "}";
            }
            else
            {
                return "{" +
                       $"{'"'}error{'"'} : {'"'}{result}{'"'} " +
                       "}";
            }
        }

        public static async Task<string> Get(string get ,string uid)
        {
            string sql;
            string resule;
            if (get.Equals("Account"))
            {
                sql = $"SELECT * FROM {get} WHERE uid = '{uid}'";
                resule = "" + await AzureResources.SqlDatabase.QueryOne(sql);
                if (resule.Equals(""))
                {
                    return "{" +
                       $"{'"'}error{'"'} : {'"'}NOT ACCOUNT{'"'} " +
                       "}";
                }
                else
                {
                    return resule;
                }

            }
            else
            {
                sql = $"SELECT * FROM {get} WHERE owner_uid = '{uid}' ";
                resule = "" + await AzureResources.SqlDatabase.QueryOne(sql);
                if (resule.Equals(""))
                {
                    return "{" +
                       $"{'"'}error{'"'} : {'"'}NOT Profile{'"'} " +
                       "}";
                }
                else
                {
                    return resule;

                }
            }
            
        }

        public static async Task<string> AddJob(string tital,string max,string min,string category,string create,string modified,
            string d1,string d2,string d3,string d4,string d5)
        {
            string sql = $"INSERT INTO Job (title,min_salary,max_salary,category,created_date,modified_date,detail_1," +
                $"detail_2,detail_3,detail_4,detail_5)" +
                $" VALUES('{tital}',{max},{min},{category},'{create}','{modified}','{d1}','{d2}','{d3}','{d4}','{d5}');";
            string result = await AzureResources.SqlDatabase.NoQuery(sql);
            if (!result.Equals("OK"))
            {
                return "{" +
                       $"{'"'}error{'"'} : {'"'}{result}{'"'} " +
                       "}";

            }
            else
            {
                return "{" +
                       $"{'"'}result{'"'} : {'"'}success{'"'} " +
                       "}";
            }
        }

        public static async Task<string> GetJob(string id)
        {
            string sql;
            string result;
            if (id.Equals("all"))
            {
                sql = $"SELECT id,title,min_salary,max_salary,category,created_date,modified_date,detail_1 FROM Job ;";
                result = await AzureResources.SqlDatabase.Query(sql);
                if (result.Equals("[]"))
                {
                    return "{" +
                       $"{'"'}error{'"'} : {'"'}Not Job{'"'} " +
                       "}";
                }
                else
                {
                    return result;
                }
            }
            else
            {
                sql = $"SELECT * FROM Job WHERE id = {id};";
                result = ""+await AzureResources.SqlDatabase.QueryOne(sql);
                if (result.Equals(""))
                {
                    return "{" +
                       $"{'"'}error{'"'} : {'"'}Not Job{'"'} " +
                       "}";
                }
                else
                {
                    return result;
                }

            }
            
        }

        public static async Task<string> AddCategory(string name)
        {
            string sql = $"INSERT INTO JobCategory (name) VALUES ('{name}') ;";
            string result = await AzureResources.SqlDatabase.NoQuery(sql);
            if (!result.Equals("OK"))
            {
                return "{" +
                      $"{'"'}error{'"'} : {'"'}{result}{'"'} " +
                      "}";
            }
            else
            {
                sql = "SELECT TOP(1) id FROM JobCategory ORDER BY id DESC ;";
                result = await AzureResources.SqlDatabase.QueryOne(sql);
                return "{" +
                    $"{'"'}category_add{'"'} : {'"'}success{'"'} ," +
                    $"{'"'}category{'"'} : " +
                        "{ " +
                        $"{result.Substring(1,result.Length-2)} " +
                        $"," +
                        $"{'"'}name{'"'} : {'"'}{name}{'"'}" +
                        "}" +
                    "}";
            }
            
        }

        public static async Task<string> GetCategory(string id)
        {
            string sql;
            string result;
            if (id.Equals("all"))
            {
                sql = "SELECT * FROM JobCategory ;";
                result = await AzureResources.SqlDatabase.Query(sql);
                if (result.Equals("[]"))
                {
                    return "{" +
                        $"{'"'}error{'"'} : {'"'}Not JobCategory Data{'"'} " +
                        "}";
                }
                else
                {
                    return result;
                }
            }
            else
            {
                sql = $"SELECT * FROM JobCategory WHERE id = {id} ";
                result = "" + await AzureResources.SqlDatabase.QueryOne(sql);
                if (result.Equals(""))
                {
                    return "{" +
                        $"{'"'}error{'"'} : {'"'}Not JobCategory id = {id} . {'"'} " +
                        "}";

                }
                else if (result.Equals("ERROR"))
                {
                    return "{" +
                        $"{'"'}error{'"'} : {'"'}ERROR: INPUT id = '{id}' .{'"'} " +
                        "}";

                }
                else
                {
                    return result;
                }
            }
        }

        public static async Task<string> DeleteCategory(string id)
        {
            string sql = $"SELECT id FROM JobCategory WHERE id = {id} ;";
            string result=""+ await AzureResources.SqlDatabase.QueryOne(sql);
            if (result.Equals(""))
            {
                return "{" +
                         $"{'"'}error{'"'} : {'"'}Not JobCategory id = {id} . {'"'} " +
                         "}";
            }
            else
            {
                sql = $"DELETE FROM JobCategory WHERE id = {id}";
                result = await AzureResources.SqlDatabase.NoQuery(sql);
                if (!result.Equals("OK"))
                {
                    return "{" +
                     $"{'"'}error{'"'} : {'"'}{result}{'"'} " +
                     "}";
                }
                else
                {
                    return "{" +
                     $"{'"'}category_del{'"'} : {'"'}success{'"'} " +
                     "}";
                }
            }

        }
    }
}
