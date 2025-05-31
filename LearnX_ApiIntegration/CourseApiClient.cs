using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Courses;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration
{
    public class CourseApiClient : BaseApiClient, ICourseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Create(CourseRequest course)
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
                requestContent.Add(new StringContent(course.CourseName ?? string.Empty), nameof(course.CourseName));
                requestContent.Add(new StringContent(course.Description ?? string.Empty), nameof(course.Description));
                requestContent.Add(new StringContent(course.InstructorID.ToString()), nameof(course.InstructorID));
                requestContent.Add(new StringContent(course.CategoryID.ToString()), nameof(course.CategoryID));
                requestContent.Add(new StringContent(course.StartDate.ToString("o")), nameof(course.StartDate)); // ISO 8601 format
                requestContent.Add(new StringContent(course.EndDate.ToString("o")), nameof(course.EndDate));
                requestContent.Add(new StringContent(course.Price.ToString("F2")), nameof(course.Price)); // Format decimal to 2 decimal places

                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync("api/course", requestContent);

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


        public async Task<bool> Delete(int id)
        {
            return await Delete($"api/course/{id}");
        }

        public Task<bool> Edit(Guid id, Course course)
        {
            throw new NotImplementedException();
        }

        public Task<List<Course>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Course> GetbyID(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<MyCourses> GetmyCourse(Guid id)
        {
            var data = await GetAsync<MyCourses>($"api/course/user/{id}");
            return data;

        }
        public async Task<List<AppUser>> GetUserCourse(int idCourse)
        {
            var data = await GetListAsync<AppUser>($"api/Course/courseUser/{idCourse}");
            return data;

        }

    }
}