using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.PayMent;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LearnX_ApiIntegration.PayMent
{
    public class PaymentApiClient : BaseApiClient, IPaymentApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<PaymentApiClient> _logger;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentApiClient(HttpClient http, ILogger<PaymentApiClient> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _http = http;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PaymentCreatedResponse?> CreatePaymentAsync(CreatePaymentRequest request)
        {
            try
            {
                var sessions = _httpContextAccessor
                                .HttpContext
                                .Session
                                .GetString(SystemConstants.AppSettings.Token);
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:5190");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Payment/Create", httpContent);
                // POST to PaymentService: /api/Payment/Create
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("CreatePaymentAsync failed: {Status} {Reason}", response.StatusCode, response.ReasonPhrase);
                    return null;
                }
                var result = await response.Content.ReadFromJsonAsync<PaymentCreatedResponse>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreatePaymentAsync error");
                return null;
            }
        }

        public async Task<PaymentStatusResponse?> GetPaymentStatusAsync(string orderId)
        {
            try
            {
                var resp = await _http.GetAsync($"api/Payment/Status/{Uri.EscapeDataString(orderId)}");
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GetPaymentStatusAsync failed: {Status} {Reason}", resp.StatusCode, resp.ReasonPhrase);
                    return null;
                }
                var result = await resp.Content.ReadFromJsonAsync<PaymentStatusResponse>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaymentStatusAsync error");
                return null;
            }
        }
    }
}