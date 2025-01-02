using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.Courses;
using LearnX_ModelView.Catalog.Enrollment;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LearnX_ApiIntegration
{
    public class EnrollmentClient : BaseApiClient, IEnrollmentClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EnrollmentClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Create(EnrollmentRequest enrollmentRequest)
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
                requestContent.Add(new StringContent(enrollmentRequest.UserID.ToString()), "UserID");
                requestContent.Add(new StringContent(enrollmentRequest.CourseID.ToString()), "CourseID");
                requestContent.Add(new StringContent(enrollmentRequest.EnrollmentDate.ToString("yyyy-MM-ddTHH:mm:ss")), "EnrollmentDate");

                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync("api/Enrollment", requestContent);

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

        public async Task<MyCourses> GetmyCourse(Guid id)
        {
            var data = await GetAsync<MyCourses>($"api/course/user/{id}");
            return data;

        }
        public async Task<bool> Outclass(OutclassRequest enrollment)
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
                requestContent.Add(new StringContent(enrollment.CourseID.ToString()), "CourseID");
                requestContent.Add(new StringContent(enrollment.UserID.ToString()), "UserID");
                // Gửi yêu cầu POST tới API
                var request = new HttpRequestMessage(HttpMethod.Delete, "api/Enrollment")
                {
                    Content = requestContent
                };
                var response = await client.SendAsync(request);


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

    }
}