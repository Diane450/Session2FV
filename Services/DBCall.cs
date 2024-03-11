using DynamicData;
using Newtonsoft.Json;
using Session2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Session2v2.Services
{
    public static class DBCall
    {
        private static HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5159/api/Subdepartment")
        };


        /// <summary>
        /// Авторизация пользователя в программе через код, который содержит только цифры
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> AuthorizeAsync(string code)
        {
            string dataSerialized = JsonConvert.SerializeObject(code);
            StringContent serializedContent = new StringContent(dataSerialized, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress+ "/Authorization", serializedContent);
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(answer);
            }
            throw new Exception();
        }

        public static async Task<List<Request>> GetAllRequestsAsync()
        {
            List<Request> requests = new List<Request>();
            List<GroupRequest> groupRequests = await GetGroupRequestsAsync();
            List<PrivateRequest> privateRequests = await GetPrivateRequestsAsync();
            requests.AddRange(groupRequests);
            requests.AddRange(privateRequests);
            return requests;
        }

        private static async Task<List<PrivateRequest>> GetPrivateRequestsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetPrivateRequests");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                List<PrivateRequest> privateRequest = JsonConvert.DeserializeObject<List<PrivateRequest>>(answer);
                PrivateRequest.ConvertByteToBitmap(privateRequest);
                return JsonConvert.DeserializeObject<List<PrivateRequest>>(answer);
            }
            throw new Exception();
        }

        private static async Task<List<GroupRequest>> GetGroupRequestsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetGroupRequests");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                List<GroupRequest> groupRequest = JsonConvert.DeserializeObject<List<GroupRequest>>(answer);
                GroupRequest.ConvertByteToBitmap(groupRequest);
                return groupRequest;
            }
            throw new Exception();
        }
    }
}
