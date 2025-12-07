using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.PayMent;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace LearnX_ApiIntegration.PayMent
{
    public class PaymentApiClient : IPaymentApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<PaymentApiClient> _logger;

        public PaymentApiClient(HttpClient http, ILogger<PaymentApiClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<PaymentCreatedResponse?> CreatePaymentAsync(CreatePaymentRequest request)
        {
            try
            {
                // POST to PaymentService: /api/Payment/Create
                var resp = await _http.PostAsJsonAsync("api/Payment/Create", request);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("CreatePaymentAsync failed: {Status} {Reason}", resp.StatusCode, resp.ReasonPhrase);
                    return null;
                }
                var result = await resp.Content.ReadFromJsonAsync<PaymentCreatedResponse>();
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