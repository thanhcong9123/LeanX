using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;
using LearnX_ModelView.Common;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LearnX_ApiIntegration
{
    public class ScoreApiClient : BaseApiClient, IScoreApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ScoreApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> AddScoreAsync(ScoreRequest scoreRequest)
        {
            try
            {
                // Lấy Token từ Session (nếu có)
                 var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
                client.BaseAddress = new Uri("http://localhost:5041");
                var requestContent = new MultipartFormDataContent();
        
                // Tạo nội dung yêu cầu để gửi tới API
                requestContent.Add(new StringContent(scoreRequest.IdUser.ToString()), "IdUser");
                requestContent.Add(new StringContent(scoreRequest.ExerciseId.ToString()), "ExerciseId");
                requestContent.Add(new StringContent(scoreRequest.DateCompleted.ToString("yyyy-MM-ddTHH:mm:ss")), "DateCompleted");
                requestContent.Add(new StringContent(scoreRequest.Score.ToString()), "Score");
                requestContent.Add(new StringContent(scoreRequest.IsPassed.ToString()), "IsPassed");

                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync("api/Score", requestContent);
                // Kiểm tra kết quả trả về
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu cần)
                Console.WriteLine($"Error creating score: {ex.Message}");
                return false;
            }

        }

        public async Task<List<ExerciseScoreDto>> GetScoreAsync(Guid userId)
        {
            var data = await GetListAsync<ExerciseScoreDto>($"api/Score?idUser={userId}");
            return data;
        }
    }
}