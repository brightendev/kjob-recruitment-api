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

            string email = req.Query["email"];
            string password = req.Query["password"];

            return await Api.Account.CreateAccount(email, password);
        }

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
    }

}
