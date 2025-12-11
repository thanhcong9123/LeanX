using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LearnX_Application.ApiIntegration.Momo;
using Microsoft.Extensions.Options;

namespace LearnX_Application.Comman.PayMent
{
        public class MomoClient : IMomoClient
        {
            private readonly MomoOptions _momoOptions;
            private readonly HttpClient _httpClient;
            public MomoClient(IOptions<MomoOptions> settings, HttpClient httpClient)
            {
                _momoOptions = settings.Value;
                _httpClient = httpClient;
            }
            public async Task<MomoCreatePaymentResponse> CreatePaymentAsync(MomoCreatePaymentRequest request)
            {
                var requestId = Guid.NewGuid().ToString();
                var amount = ((long)request.Amount).ToString();
                var rawHash = $"accessKey={_momoOptions.AccessKey}" +
                            $"&amount={amount}" +
                            $"&extraData={request.ExtraData}" +
                            $"&ipnUrl={request.NotifyUrl}" +
                            $"&orderId={request.OrderCode}" +
                            $"&orderInfo={request.OrderInfo}" +
                            $"&partnerCode={_momoOptions.PartnerCode}" +
                            $"&redirectUrl={request.ReturnUrl}" +
                            $"&requestId={requestId}" +
                            $"&requestType={_momoOptions.RequestType}";
                var signature = ComputeHmacSha256(rawHash, _momoOptions.SecretKey);
                var payload =  new
                {
                    partnerCode =  _momoOptions.PartnerCode,
                    partnerName = "LearnX",
                    storeId = _momoOptions.PartnerCode,
                    requestId = requestId,
                    amount = amount,
                    orderId = request.OrderCode,
                    orderInfo = request.OrderInfo,
                    redirectUrl = request.ReturnUrl,
                    ipnUrl = request.NotifyUrl,
                    lang = "vi",
                    extraData = request.ExtraData,
                    requestType = _momoOptions.RequestType,
                    signature = signature
                };
                var json =  JsonSerializer.Serialize(payload);
                var conetnt =  new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    var response = await _httpClient.PostAsync(_momoOptions.MomoApiUrl ,conetnt);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var momoResponse =  JsonSerializer.Deserialize<MomoCreatePaymentResponse>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return momoResponse ?? new MomoCreatePaymentResponse
                    {
                        ResultCode = -1,
                        Message = "Failed to parse response from Momo"
                    };
                }
                catch (System.Exception ex)
                {
                    
                    return new MomoCreatePaymentResponse
                    {
                        ResultCode = -1,
                        Message = $"Error calling MoMo API: {ex.Message}"
                    };
                }

            }
            public bool VerifySignature(string rawData, string signature)
            {
                var computedSignature = ComputeHmacSha256(rawData, _momoOptions.SecretKey);
                return computedSignature.Equals(signature, StringComparison.OrdinalIgnoreCase);
            }
            private string ComputeHmacSha256(string message, string secretKey)
            {
                var keyBytes = Encoding.UTF8.GetBytes(secretKey);
                var messageBytes = Encoding.UTF8.GetBytes(message);

                using var hmac = new HMACSHA256(keyBytes);
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
}