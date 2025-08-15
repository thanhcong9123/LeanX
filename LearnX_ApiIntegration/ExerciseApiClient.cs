using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Common;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration
{
    public class ExerciseApiClient : BaseApiClient, IExerciseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExerciseApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<string>> AddExerciseAsync(ExerciseRequestWrapper model)
        {

            // Lấy token từ Session
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
                var json = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/Exercise", httpContent);

                var responseString = await response.Content.ReadAsStringAsync();

                // Kiểm tra kết quả
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(responseString) ?? new ApiSuccessResult<string> { ResultObj = null };
                }
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(responseString) ?? new ApiErrorResult<string> { ResultObj = null };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string> { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<List<Exercise>> GetAll(Guid id)
        {
            var data = await GetListAsync<Exercise>($"/api/Exercise/user/{id}/exercises");
            return data;
        }
        public async Task<List<Question>> getQuestion(int id)
        {
            var data = await GetListAsync<Question>($"api/Exercise/question/{id}");
            return data;
        }

        public async Task<List<Exercise>> GetExerciseDetailsAsync(int courseId)
        {
            var data = await GetListAsync<Exercise>($"/api/Exercise/exercise/{courseId}");
            return data;
        }

        public async Task<bool> DeleteExerciseAsync(int courseId)
        {
            return await Delete($"http://localhost:5041/api/Exercise/{courseId}");
        }

        // Thêm method để kiểm tra loại bài tập (trắc nghiệm hay tự luận)
        public async Task<bool> IsEssayExercise(int exerciseId)
        {
            try
            {
                // Logic để xác định bài tập có phải là tự luận không
                // Có thể dựa vào việc kiểm tra có Questions hay không
                var questions = await getQuestion(exerciseId);
                return questions == null || !questions.Any();
            }
            catch
            {
                return false;
            }
        }

        // Method để lấy thông tin exercise đầy đủ
        public async Task<Exercise?> GetExerciseByIdAsync(int exerciseId)
        {
            var data = await GetAsync<Exercise>($"/api/Exercise/{exerciseId}");
            return data;
        }
    }
}