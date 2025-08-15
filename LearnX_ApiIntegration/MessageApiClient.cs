using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Messages;
using LearnX_ModelView.Common;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration
{
    public class MessageApiClient : BaseApiClient, IMessageApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MessageApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        // Gửi tin nhắn
        public async Task<bool> SendMessageAsync(SendMessageRequest request)
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
                requestContent.Add(new StringContent(request.SenderId.ToString()), "SenderId");
                requestContent.Add(new StringContent(request.ReceiverId.ToString()), "ReceiverId");
                requestContent.Add(new StringContent(request.Content.ToString()), "Content");

                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync("api/Messages", requestContent);
                Console.WriteLine("SendMessageAsync  " + response.IsSuccessStatusCode);

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

        // Lấy tất cả tin nhắn của người dùng
        public async Task<List<ViewMessage>> GetMessagesAsync(Guid userId)
        {
            var data = await GetListAsync<ViewMessage>($"/api/Messages?userId={userId}");

            return data;
        }

        // Đánh dấu tin nhắn là đã đọc
        public async Task<ApiResult<string>> MarkMessageAsReadAsync(int messageId)
        {
            try
            {

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
                var json = JsonConvert.SerializeObject(messageId);

                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsJsonAsync($"/api/Messages/mark-as-read?messageId={messageId}", httpContent);
                
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("MarkMessageAsReadAsync  " + messageId);
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

        public async Task<Guid?> GetUserIdByEmailAsync(string email)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var response = await client.GetAsync($"/api/Messages/get-user-id-by-email?email={Uri.EscapeDataString(email)}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (Guid.TryParse(responseString.Trim('"'), out var userId))
                    {
                        return userId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user ID by email: {ex.Message}");
                return null;
            }
        }
    }
}