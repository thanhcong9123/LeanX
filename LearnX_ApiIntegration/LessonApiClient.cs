using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using LearnX_Data.Entities;
using LearnX_Utilities.Constants;
using LearnX_ModelView.Common;
using System.Text;
using LearnX_ModelView.Catalog.Lessons;

namespace LearnX_ApiIntegration
{
    public class LessonApiClient : BaseApiClient, ILessonApiClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LessonApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Lesson>> GetAllLessonsAsync(int courseId)
        {
            var response = await GetListAsync<Lesson>($"http://localhost:5041/api/Lesson/lesson/{courseId}");

            return response;
        }

        public async Task<bool> AddLessonAsync(LessonRequest lesson)
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

                // Thêm dữ liệu vào form-data
                requestContent.Add(new StringContent(lesson.CourseID.ToString()), "CourseID");
                requestContent.Add(new StringContent(lesson.LessonTitle ?? string.Empty), "LessonTitle");
                requestContent.Add(new StringContent(lesson.Content ?? string.Empty), "Content");
                requestContent.Add(new StringContent(lesson.Objectives ?? string.Empty), "Objectives");
                requestContent.Add(new StringContent(lesson.VideoUrl ?? string.Empty), "VideoUrl");
                requestContent.Add(new StringContent(lesson.StratDate.ToString("o")), "StratDate");
                requestContent.Add(new StringContent(lesson.EndDate.ToString("o")), "EndDate");
                if (lesson.Resources != null && lesson.Resources.Count > 0)
                {
                    for (int i = 0; i < lesson.Resources.Count; i++)
                    {
                        var resource = lesson.Resources[i];
                        requestContent.Add(new StringContent(resource.ResourceName ?? string.Empty), $"Resources[{i}].ResourceTitle");
                        requestContent.Add(new StringContent(resource.ResourceUrl ?? string.Empty), $"Resources[{i}].ResourceUrl");
                        requestContent.Add(new StringContent(resource.ResourceType ?? string.Empty), $"Resources[{i}].ResourceType");
                    }
                }
                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync("api/Lesson", requestContent);

                // Kiểm tra kết quả trả về
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu cần)
                Console.WriteLine($"Error creating course: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLessonAsync(int lessonId)
        {
            return await Delete($"http://localhost:5041/api/Lesson/{lessonId}");

        }



    }

}
