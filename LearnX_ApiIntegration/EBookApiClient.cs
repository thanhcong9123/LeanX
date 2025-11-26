using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_Data.Migrations;
using LearnX_ModelView.Catalog.EBook;
using LearnX_ModelView.Common;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

        public async Task<ApiResult<string>> UploadBookAsync(EBookRequest eBookRequest)
        {
            try
            {

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

                var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var json = JsonConvert.SerializeObject(eBookRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/books", httpContent);


                // Gửi yêu cầu POST tới API
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from API: " + responseString);
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

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await Delete($"api/books/{id}"); ;
        }
    }


}