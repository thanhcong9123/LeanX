using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_Data.Migrations;
using LearnX_ModelView.Catalog.EBook;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LearnX_ApiIntegration
{
    public class EBookApiClient : BaseApiClient, IEBookApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EBookApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<EBook>> GetBooksAsync()
        {
            return await GetListAsync<EBook>("api/books");
        }

        public async Task<EBook> GetBookByIdAsync(int id)
        {
            var response = await GetAsync<EBook>($"api/books/{id}");
            Console.WriteLine("GetBookByIdAsync đã nhận " + response.LinkYoutube);
      
            return response;
        }

        public async Task<bool> UploadBookAsync(EBookRequest eBookRequest)
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
                requestContent.Add(new StringContent(eBookRequest.Title), "title");
                requestContent.Add(new StringContent(eBookRequest.Description), "description");
                requestContent.Add(new StringContent(eBookRequest.imgPath), "imgPath");
                requestContent.Add(new StringContent(eBookRequest.FilePath), "FilePath");


                // Gửi yêu cầu POST tới API
                var response = await client.PostAsync("api/books", requestContent);

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

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await Delete($"api/books/{id}");;
        }
    }


}