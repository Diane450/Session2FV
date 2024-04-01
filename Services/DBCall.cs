using DynamicData;
using Newtonsoft.Json;
using Session2v2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Session2v2.Services
{
    /// <summary>
    /// класс со статическими методами-запросами к web-api
    /// </summary>
    public static class DBCall
    {
        private static readonly HttpClient _client = new()
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
            StringContent serializedContent = new(dataSerialized, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/Authorization", serializedContent);
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(answer);
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает абсолютно все заявки из бд 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Request>> GetAllRequestsAsync()
        {
            List<Request> requests = new();
            List<GroupRequest> groupRequests = await GetGroupRequestsAsync();
            List<PrivateRequest> privateRequests = await GetPrivateRequestsAsync();
            requests.AddRange(groupRequests);
            requests.AddRange(privateRequests);
            return requests;
        }

        /// <summary>
        /// Возвращает список личных заявок из бд
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task<List<PrivateRequest>> GetPrivateRequestsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetPrivateRequests");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                List<PrivateRequest> privateRequest = JsonConvert.DeserializeObject<List<PrivateRequest>>(answer);
                //PrivateRequest.ConvertByteToBitmap(privateRequest);
                return privateRequest;
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает список групповых заявок из бд
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task<List<GroupRequest>> GetGroupRequestsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetGroupRequests");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                List<GroupRequest> groupRequest = JsonConvert.DeserializeObject<List<GroupRequest>>(answer);
                //GroupRequest.ConvertByteToBitmap(groupRequest);
                return groupRequest;
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает список отделов
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<ObservableCollection<Department>> GetDepartmentsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetDepartments");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                ObservableCollection<Department> departments = JsonConvert.DeserializeObject<ObservableCollection<Department>>(answer);
                return departments;
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает список статусов заявки
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<ObservableCollection<Status>> GetStatusesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetStatuses");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                ObservableCollection<Status> statuses = JsonConvert.DeserializeObject<ObservableCollection<Status>>(answer);
                return statuses;
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает список типов заявок
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<ObservableCollection<MeetingType>> GetMeetingTypesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetMeetingTypes");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                ObservableCollection<MeetingType> statuses = JsonConvert.DeserializeObject<ObservableCollection<MeetingType>>(answer);
                return statuses;
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает список причин отказа заявок
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public static async Task<ObservableCollection<DeniedReason>> GetDeniedReasonsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/GetDeniedReasons");
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                ObservableCollection<DeniedReason> statuses = JsonConvert.DeserializeObject<ObservableCollection<DeniedReason>>(answer);
                return statuses;
            }
            throw new Exception();
        }

        /// <summary>
        /// Проверяет, находится ли гость в черном списке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> IsGuestBlackListedAsync(int id)
        {
            string dataSerialized = JsonConvert.SerializeObject(id);
            StringContent serializedContent = new(dataSerialized, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/IsGuestBlackListed", serializedContent);
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(answer);
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает причину, по которой был отклонена личная заявка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<DeniedReason> GetPrivateRequestDeniedReasonAsync(int id)
        {
            string dataSerialized = JsonConvert.SerializeObject(id);
            StringContent serializedContent = new(dataSerialized, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/GetPrivateRequestDeniedReason", serializedContent);
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<DeniedReason>(answer);
            }
            throw new Exception();
        }

        /// <summary>
        /// Вовращает причину, по которой была отклонена групповая заявка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<DeniedReason> GetGroupRequestDeniedReasonAsync(int id)
        {
            string dataSerialized = JsonConvert.SerializeObject(id);
            StringContent serializedContent = new(dataSerialized, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/GetGroupRequestDeniedReason", serializedContent);
            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<DeniedReason>(answer);
            }
            throw new Exception();
        }
    }
}