using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor for dependency injection
        public BaseApiClient(IHttpClientFactory httpClientFactory,
                             IHttpContextAccessor httpContextAccessor,
                             IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        // Method to send GET request and return response of type TResponse
        protected async Task<TResponse> GetAsync<TResponse>(string url)
        {
                var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            var baseAddress = _configuration[SystemConstants.AppSettings.BaseAddress];

            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(nameof(baseAddress), "BaseAddress configuration is missing.");
            }
            client.BaseAddress = new Uri(baseAddress);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(body);
            }

            // Handle error response (you may want to log or throw a specific exception)
            throw new Exception($"API call failed: {body}");
        }

        // Method to get a list of items from the API
        public async Task<List<T>> GetListAsync<T>(string url)
        {
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);
         

            var client = _httpClientFactory.CreateClient();
            var baseAddress = _configuration[SystemConstants.AppSettings.BaseAddress];

            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(nameof(baseAddress), "BaseAddress configuration is missing.");
            }
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<T>>(body);
            }

            // Handle error response (you may want to log or throw a specific exception)
            throw new Exception($"API call failed: {body}");
        }

        // Method to delete an item via API
        public async Task<bool> Delete(string url)
        {
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);
         

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
