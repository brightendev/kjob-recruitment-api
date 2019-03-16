﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JobRecruitmentApi.AzureResources
{
    static class ActiveDirectory
    {
        private static readonly string tenant = "jobrecruitment.onmicrosoft.com";
        private static readonly string clientID = "9a82b6ec-a884-4294-b355-0f4f59ea9444";
        private static readonly string clientSecret = "!Iz]}j@>=*HL|4--?$elu:jFix44i:";

        private static readonly HttpClient httpClient = new HttpClient();


        private static async Task<string> getAccessTokenForGraph() {

            string url = $"https://login.microsoftonline.com/{tenant}/oauth2/token";
            string apiResource = "https://graph.microsoft.com/";

            HttpContent requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientID },
                { "client_secret", clientSecret },
                { "resource", apiResource }
            });

            HttpResponseMessage response = await httpClient.PostAsync(url, requestContent);

            if (response.StatusCode == HttpStatusCode.OK) {

                string respJson = await response.Content.ReadAsStringAsync();
                JObject respJsonObject = JsonConvert.DeserializeObject(respJson) as JObject;
                string accessToken = respJsonObject["access_token"].Value<string>();

                return accessToken;
            }
            return await response.Content.ReadAsStringAsync();   // error
        }

        public static async Task<string> CreateAccount(string username, string password) {

            string url = "https://graph.microsoft.com/v1.0/users";
            string apiAuthToken = await getAccessTokenForGraph();

            var requestPayload = new
            {
                accountEnabled = "true",
                displayName = username,
                mailNickname = "mail",
                userPrincipalName = $"{username}@{tenant}",
                passwordProfile = new {
                    forceChangePasswordNextSignIn = "false",
                    password = password
                }
            };

            HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = requestContent,
                Headers = {
                    Authorization = new AuthenticationHeaderValue("Bearer", apiAuthToken),
                }
            };

            HttpResponseMessage response = await httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> TryToGetUserAccessToken(string username, string password) {

            string url = $"https://login.microsoftonline.com/{tenant}/oauth2/token";
            string apiResource = "https://graph.microsoft.com/";

            HttpContent requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", clientID },
                { "client_secret", clientSecret },
                { "username", $"{username}@{tenant}" },
                { "password", password},
                { "resource", apiResource }
            });

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) {
                Content = requestContent
            };

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string respJson = await response.Content.ReadAsStringAsync();
                JObject respJsonObject = JsonConvert.DeserializeObject(respJson) as JObject;
                string accessToken = respJsonObject["access_token"].Value<string>();

                return accessToken;
            }

            if(areUserCredentialsIncorrect(await response.Content.ReadAsStringAsync())) {
                return "AADSTS50126";
            }

            return await response.Content.ReadAsStringAsync();  // error
        }

        public static async Task<string> SignInWithToken(string token) {

            string url = "https://graph.microsoft.com/v1.0/me";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers = {
                    Authorization = new AuthenticationHeaderValue("Bearer", token),
                }
            };

            HttpResponseMessage response = await httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        private static bool areUserCredentialsIncorrect(string payload) {

            if(payload.Contains("AADSTS50126")) return true;
            return false;
        }
    }
}
