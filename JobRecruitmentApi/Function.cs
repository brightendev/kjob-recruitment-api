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

            return await Api.Account.UserSignin(email, password);
        }

        [FunctionName("IsAccountExist")]
        public static async Task<char> IsAccountExist(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)

        {
            log.LogInformation("IsAccountExist Function has been called");

            string email = req.Query["email"];

            return await Api.Account.IsAccountExist(email);
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

        [FunctionName("CreateProfile")]
        public static async Task<string> createProfile(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string personal_id = req.Query["personal_id"];
            string thai_name = req.Query["thai_name"];
            string eng_name = req.Query["eng_name"];
            string date_of_birth = req.Query["date_of_birth"];
            string nationality = req.Query["nationality"];
            string race = req.Query["race"];
            string religion = req.Query["religion"];
            string blood = req.Query["blood"];
            string relationship = req.Query["relationship"];
            string child = req.Query["child"];
            string military_criterion = req.Query["military_criterion"];
            string address = req.Query["address"];
            string province = req.Query["province"];
            string telephone = req.Query["telephone"];
            string email = req.Query["email"];
            string owner_uid = req.Query["owner_uid"];
            string gender = req.Query["gender"];
            return await Api.Database.CreateProfile(personal_id,thai_name,eng_name,date_of_birth,nationality,
                race,religion,blood,relationship,child,military_criterion,address,province,telephone,email,owner_uid,gender);
        }

        [FunctionName("CheckRole")]
        public static async Task<string> showRole(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string uid = req.Query["uid"];
            return await Api.Database.CheckRole(uid);
        }
 
        [FunctionName("CheckProfile")]
        public static async Task<string> CheckProfule(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
          ILogger log)
        {
            string uid = req.Query["uid"];
            return await Api.Database.CheckProfile(uid);
        }
        
        [FunctionName("GetPublicData")]
        public static async Task<string> GetPublic(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
          ILogger log) => await Api.Database.getPublic();

        [FunctionName("Religion")]
        public static async Task<string> religion(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log) => await Api.Database.GetReligion();

        [FunctionName("Blood")]
        public static async Task<string> blood(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log) => await Api.Database.GetBlood();

        [FunctionName("Relationship")]
        public static async Task<string> relationship(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log) => await Api.Database.GetRelationship();

        [FunctionName("MilitaryCriterion")]
        public static async Task<string> MilitaryCriterion(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log) => await Api.Database.GetMilitaryCriterion();

        [FunctionName("Province")]
        public static async Task<string> province(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
          ILogger log) => await Api.Database.GetProvince();

        [FunctionName("GetAccount")]
        public static async Task<string> getAccount(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string email = req.Query["email"];
            return await Api.Database.GetAccount(email);
        }

        [FunctionName("GetAccountData")]
        public static async Task<string> accountData(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string uid = req.Query["uid"];
            return await Api.Database.AccountData(uid);
        }
        
        [FunctionName("EditUser")]
        public static async Task<string> editUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string edit = req.Query["edit"];
            string value = req.Query["value"];
            string uid = req.Query["uid"];
            string st = "set";
            if (st.Equals(edit.Substring(0,3)))
            {
                return await Api.Database.EditUserNotif(edit, value, uid);
            }
            else
            {
                return await Api.Database.EditProfile(edit, value, uid);
            }

        }

        [FunctionName("GetUser")]
        public static async Task<string> getUser(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string get = req.Query["get"];
            string uid = req.Query["uid"];
            return await Api.Database.Get(get,uid);
        }

        [FunctionName("AddJob")]
        public static async Task<string> addJob(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string title = req.Query["title"];
            string min_salary = req.Query["min_salary"];
            string max_salary = req.Query["max_salary"];
            string category = req.Query["category"];
            string created_date = req.Query["created_date"];
            string modified_date = req.Query["modified_date"];
            string detail_1 = req.Query["detail_1"];
            string detail_2 = req.Query["detail_2"];
            string detail_3 = req.Query["detail_3"];
            string detail_4 = req.Query["detail_4"];
            string detail_5 = req.Query["detail_5"];
            return await Api.Database.AddJob(title,min_salary,max_salary,category,created_date,modified_date,
                detail_1,detail_2,detail_3,detail_4,detail_5);
        }

        [FunctionName("GetJob")]
        public static async Task<string> getJob(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string job = req.Query["job"];
            return await Api.Database.GetJob(job);
        }

        [FunctionName("AddCategory")]
        public static async Task<string> addCategory(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string name = req.Query["name"];
            return await Api.Database.AddCategory(name);
        }

        [FunctionName("GetCategory")]
        public static async Task<string> getCategory(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string id = req.Query["category"];
            return await Api.Database.GetCategory(id);
        }

        [FunctionName("DeleteCategory")]
        public static async Task<string> deleteCategory(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string id = req.Query["id"];
            return await Api.Database.DeleteCategory(id);
        }
    }

}
