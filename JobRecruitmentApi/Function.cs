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

        // return role of the given uid account
        [FunctionName("CheckRole")]
        public static async Task<string> showRole(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string uid = req.Query["uid"];
            return await Api.Database.CheckRole(uid);
        }

        // check for the existence of Profile Data which is owned by given uid account
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

        // return all accounts in database use for dashboard admin page (check the request query string for email to allow this function to be execute only by admin)
        [FunctionName("GetAccount")]
        public static async Task<string> getAccount(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string email = req.Query["email"];
            return await Api.Database.GetAccount(email);
        }
        // Change Role of given email account 
        [FunctionName("ChangeRole")]
        public static async Task<string> ChangeRoleOfAccount(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string email = req.Query["email"];
            string role = req.Query["role"];

            if(!Int32.TryParse(role, out int intRole) || intRole > 3 || intRole < 1) return @"{""error"":""invalid_role""}";

            return await Api.Database.ChangeRole(email, role);
        }


        // <obsolete> return account and profile information of the owner of given uid
        [FunctionName("GetAccountData")]
        public static async Task<string> accountData(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string uid = req.Query["uid"];
            return await Api.Database.AccountData(uid);
        }
        
        // ============== Function for user data (account, profile, ...) ===========
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
        // =========== #END Function for user data (account, profile, ...) =============
        
        // ==================== Manipulation of Job Table ==================
        [FunctionName("AddJob")]
        public static async Task<string> addJob(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
        /*    string title = req.Query["title"];
            string min_salary = req.Query["min_salary"];
            string max_salary = req.Query["max_salary"];
            string category = req.Query["category"];
            string created_date = req.Query["created_date"];
            string modified_date = req.Query["modified_date"];
            string detail_1 = req.Query["detail_1"];
            string detail_2 = req.Query["detail_2"];
            string detail_3 = req.Query["detail_3"];
            string detail_4 = req.Query["detail_4"];
            string detail_5 = req.Query["detail_5"];*/

            string requestBody = await (new StreamReader(req.Body)).ReadToEndAsync();
            JobPostRequest jobData = JsonConvert.DeserializeObject<JobPostRequest>(requestBody);

            string title = jobData.title;
            string min_salary = jobData.min_salary;
            string max_salary = jobData.max_salary;
            string category = jobData.category;
            string created_date = jobData.created_date;
            string modified_date = jobData.modified_date;
            string detail_1 = jobData.detail_1;
            string detail_2 = jobData.detail_2;
            string detail_3 = jobData.detail_3;
            string detail_4 = jobData.detail_4;
            string detail_5 = jobData.detail_5;

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

        [FunctionName("EditJob")]
        public static async Task<string> EditJob(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string job = req.Query["job"];
            string title = req.Query["title"];
            string min_salary = req.Query["min_salary"];
            string max_salary = req.Query["max_salary"];
            string category = req.Query["category"];
            string modified_date = req.Query["modified_date"];
            string detail_1 = req.Query["detail_1"];
            string detail_2 = req.Query["detail_2"];
            string detail_3 = req.Query["detail_3"];
            string detail_4 = req.Query["detail_4"];
            string detail_5 = req.Query["detail_5"];

            return await Api.Database.EditJob(job,title,min_salary,max_salary,category,modified_date,detail_1,detail_2,detail_3,detail_4,detail_5);
        }
        // ================= #END Manipulation of Job Table ==================

            // =========== Manipulation of JobCategory Table ===================
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
        // ======== #END Manipulation of JobCategory Table ===================

        // ============ Manipulation of Candidate Table  ==============
        [FunctionName("AddCandidate")]
        public static async Task<string> addCandidate(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string owner_id = req.Query["owner_id"];
            string candidate_id = req.Query["candidate_id"];
            string status = req.Query["status"];
            string extra_info = req.Query["extra_info"];
            string applied_job = req.Query["applied_job"];
            return await Api.Database.AddCadidate(owner_id, candidate_id, status, extra_info, applied_job);
        }

        [FunctionName("GetCandidate")]
        public static async Task<string> getCandidate(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string id = req.Query["id"];
            return await Api.Database.GetCadidate(id);
        }

        [FunctionName("DeleteCandidate")]
        public static async Task<string> deleteCandidate(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string id = req.Query["id"];
            return await Api.Database.DeleteCandidate(id);
        }
        // ========== #END Manipulation of Candidate Table  ==============

        // === class for store job data
        public class JobPostRequest
        {
            public string title { get; set; }
            public string min_salary { get; set; }
            public string max_salary { get; set; }
            public string category { get; set; }
            public string created_date { get; set; }
            public string modified_date { get; set; }
            public string detail_1 { get; set; }
            public string detail_2 { get; set; }
            public string detail_3 { get; set; }
            public string detail_4 { get; set; }
            public string detail_5 { get; set; }
        }
    }

}
