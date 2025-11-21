using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;
using LearnX_ModelView.Common;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var json = JsonConvert.SerializeObject(scoreRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/Score", httpContent);

                var responseString = await response.Content.ReadAsStringAsync();
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