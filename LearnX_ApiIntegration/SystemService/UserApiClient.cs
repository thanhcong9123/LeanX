using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LearnX_ModelView.Common;
using LearnX_ModelView.System.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration.SystemService
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<string>> Authenticate(Login loginRequest)
        {
            var json = JsonConvert.SerializeObject(loginRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5041");
            var response = await client.PostAsync("/api/user/authenticate", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(responseString);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(responseString) ?? new ApiErrorResult<string> { ResultObj = null };
        }

        public async Task<ApiResult<bool>> DeleteAccount(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5041");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"/api/user/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body) ?? new ApiErrorResult<bool> { ResultObj = false };

        }

        public Task<ApiResult<string>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<UserVm>> GetByID(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5041");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/user/{id}");
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(body);

            }
            return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(body);

        }

        public async Task<ApiResult<string>> Register(Register register)
        {
            var json = JsonConvert.SerializeObject(register);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5041");
            var response = await client.PostAsync("api/user", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(responseString);
            }
            Console.WriteLine("Kết quả" +responseString);
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(responseString) ?? new ApiErrorResult<string> { ResultObj = null };

        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest register)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("http://localhost:5041");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(register);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/user/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result) ?? new ApiErrorResult<bool> { ResultObj = false };

        }
    }
}