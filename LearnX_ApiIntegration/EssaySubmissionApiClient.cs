using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EssaySubmission;
using LearnX_ModelView.Common;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration
{
    public class EssaySubmissionApiClient : BaseApiClient, IEssaySubmissionApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EssaySubmissionApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<string>> CreateEssaySubmissionAsync(CreateEssaySubmissionRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/EssaySubmission", httpContent);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(responseString) ??
                           new ApiSuccessResult<string> { ResultObj = "Success" };
                }
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(responseString) ??
                       new ApiErrorResult<string> { Message = "Error occurred" };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string> { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAsync(Guid userId)
        {
            var data = await GetListAsync<EssaySubmissions>($"/api/EssaySubmission/user/{userId}");
            return data;
        }

        public async Task<List<EssaySubmissions>> GetEssaySubmissionsByExerciseAsync(int exerciseId)
        {
            var data = await GetListAsync<EssaySubmissions>($"/api/EssaySubmission/exercise/{exerciseId}");
            return data;
        }

        public async Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAndExerciseAsync(Guid userId, int exerciseId)
        {
            var data = await GetListAsync<EssaySubmissions>($"/api/EssaySubmission/user/{userId}/exercise/{exerciseId}");
            return data;
        }

        public async Task<EssaySubmissionRequest?> GetEssaySubmissionByIdAsync(int id)
        {
            var data = await GetAsync<EssaySubmissionRequest>($"/api/EssaySubmission/{id}");
            return data;
        }

        public async Task<ApiResult<string>> UpdateEssaySubmissionAsync(UpdateEssaySubmissionRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"/api/EssaySubmission/{request.Id}", httpContent);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiSuccessResult<string> { ResultObj = "Success" };
                }
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(responseString) ??
                       new ApiErrorResult<string> { Message = "Error occurred" };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string> { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<bool> DeleteEssaySubmissionAsync(int id)
        {
            return await Delete($"/api/EssaySubmission/{id}");
        }

        public async Task<ApiResult<string>> UpdateStatusAsync(int id, string status)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var json = JsonConvert.SerializeObject(status);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PatchAsync($"/api/EssaySubmission/{id}/status", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiSuccessResult<string> { ResultObj = "Success" };
                }
                return new ApiErrorResult<string> { Message = "Failed to update status" };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string> { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ApiResult<string>> AddTeacherCommentAsync(int id, string comment)
        {
            try
            {
                var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
                client.BaseAddress = new Uri("http://localhost:5041");
                var requestContent = new MultipartFormDataContent();
                requestContent.Add(new StringContent(comment ?? string.Empty), nameof(comment));

                
                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync($"/api/EssaySubmission/{id}/comment", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiSuccessResult<string> { ResultObj = "Success" };
                }
                return new ApiErrorResult<string> { Message = "Failed to add comment" };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string> { Message = $"Error: {ex.Message}" };
            }
        }
    }
}
