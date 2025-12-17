using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using LearnX_ModelView.Catalog.Exercise;
using Microsoft.Extensions.Configuration;

namespace LearnX_Application.Comman.AI
{
    public class AIQuestionGenerator : IAIQuestionGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string OPENROUTER_API_URL = "https://openrouter.ai/api/v1/chat/completions";
        public AIQuestionGenerator(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenRouter:ApiKey"] ?? "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";
        }
        public async Task<AIGeneratedQuestionsResponse> GenerateQuestionsAsync(AIGenerateQuestionsRequest request)
        {
            var systemPrompt = @"Bạn là một trợ lý tạo câu hỏi trắc nghiệm. 
                                Hãy tạo câu hỏi theo định dạng JSON như sau:
                                {
                                ""questions"": [
                                    {
                                    ""questionText"": ""Câu hỏi....."",
                                    ""answers"": [
                                        { ""answerText"": ""Đáp án A"", ""isCorrect"": false },
                                        { ""answerText"": ""Đáp án B"", ""isCorrect"": true },
                                        { ""answerText"": ""Đáp án C"", ""isCorrect"": false },
                                        { ""answerText"": ""Đáp án D"", ""isCorrect"": false }
                                    ]
                                    }
                                ]
                                }

                                Mỗi câu hỏi phải có 4 đáp án, chỉ 1 đáp án đúng.
                                QUAN TRỌNG: Chỉ trả về JSON thuần túy, không có markdown code block, không có text thừa.";
            var userPrompt = $@"Tạo {request.NumberOfQuestions} câu hỏi trắc nghiệm về chủ đề: ""{request.Title}""

                                Nội dung tham khảo: {request.Content}

                                Hãy tạo câu hỏi có độ khó vừa phải, phù hợp để kiểm tra kiến thức.";
            var requestBody = new
            {
                model = "deepseek/deepseek-r1-0528:free",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                }
            };
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8

            );
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var response = await _httpClient.PostAsync(OPENROUTER_API_URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenRouter API error: {responseString}");

            }
            var apiResponse = JsonSerializer.Deserialize<OpenRouterResponse>(responseString);
            var aiContent = apiResponse?.Choices?[0]?.Message?.Content;
            if (string.IsNullOrEmpty(aiContent))
            {
                throw new Exception("OpenRouter API returned empty content.");
            }
            aiContent = aiContent.Trim();
            if (aiContent.StartsWith("```json"))
            {
                aiContent = aiContent.Substring(7);
            }
            if (aiContent.StartsWith("```"))
            {
                aiContent = aiContent.Substring(3);
            }
            if (aiContent.EndsWith("```"))
            {
                aiContent = aiContent.Substring(0, aiContent.Length - 3);
            }
            aiContent = aiContent.Trim();
            var result = JsonSerializer.Deserialize<AIGeneratedQuestionsResponse>(aiContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result ?? new AIGeneratedQuestionsResponse
            {
                Questions = new List<GeneratedQuestion>()
            };

        }
        public async Task<AIAutoScoringResponse> ScoringAsync(AIScoringRequest request)
        {
            var answerKeyContent = await DownloadFileFromUrlAsync(request.AnswerKeyUrl);
            var submissionContent = await DownloadFileFromUrlAsync(request.SubmissionFileUrl);
            if (answerKeyContent == null || answerKeyContent.Length == 0 ||
                submissionContent == null || submissionContent.Length == 0)
            {
                return new AIAutoScoringResponse
                {
                    Score = 0,
                    Status = "Error",
                    Feedback = "Không thể tải được file từ URL"
                };
            }
            var answerKeyText = ExtractTextFromDocx(answerKeyContent);
            var submissionText = ExtractTextFromDocx(submissionContent);
            if (string.IsNullOrEmpty(answerKeyText) || string.IsNullOrEmpty(submissionText))
            {
                return new AIAutoScoringResponse
                {
                    Score = 0,
                    Status = "Error",
                    Feedback = "Không thể đọc nội dung file"
                };
            }
            var scoringPromps = BuildScoringPrompt(
                answerKeyText,
                submissionText,
                request.ScoringCriteria,
                request.MaxScore
            );
            var aiResponse = await CallAIForScoringAsync(scoringPromps);
            var scoringResult = ParseAIScoringResponse(aiResponse, request.MaxScore);

            // Implement scoring logic here
            return scoringResult;
        }
        private async Task<string> CallAIForScoringAsync(string prompt)
        {
            var requestBody = new
            {
                model = "deepseek/deepseek-r1-0528:free",
                messages = new[]
                {
                    new { role = "user", content = prompt }

                },
                temperature = 0.7,
                max_tokens = 2000
            };
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var response = await _httpClient.PostAsync(OPENROUTER_API_URL, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenRouter API error: {responseContent}");
            }
            var apiResponse = JsonSerializer.Deserialize<OpenRouterResponse>(responseContent);
            var aiContent = apiResponse?.Choices?[0]?.Message?.Content;
            if (string.IsNullOrEmpty(aiContent))
            {
                throw new Exception("AI không trả về kết quả");
            }

            return aiContent.Trim();

        }
        private AIAutoScoringResponse ParseAIScoringResponse(string aiResponse, int maxScore)
        {
            try
            {
                var cleaned = aiResponse.Trim();
                var start = cleaned.IndexOf('{');
                var end = cleaned.LastIndexOf('}');
                if (start >= 0 && end > start) cleaned = cleaned.Substring(start, end - start + 1);

                cleaned = cleaned.Trim();
                using var doc = JsonDocument.Parse(cleaned);
                var root = doc.RootElement;
                var score = Math.Min(GetInt(root, "score"), maxScore);
                var percentage = GetDouble(root, "percentage");
                var feedback = GetString(root, "feedback");
                var improvement = GetString(root, "improvement");
                var strengths = new List<string>();
                if (root.TryGetProperty("strengths", out var sArr) && sArr.ValueKind == JsonValueKind.Array)
                    strengths.AddRange(sArr.EnumerateArray().Select(x => x.GetString() ?? ""));
                var weaknesses = new List<string>();
                if (root.TryGetProperty("weaknesses", out var wArr) && wArr.ValueKind == JsonValueKind.Array)
                    weaknesses.AddRange(wArr.EnumerateArray().Select(x => x.GetString() ?? ""));

                return new AIAutoScoringResponse
                {
                    Score = Math.Min(score, maxScore),
                    Percentage = Math.Round(percentage, 2),
                    Feedback = feedback,
                    Improvement = improvement,
                    Strengths = strengths,
                    Weaknesses = weaknesses,
                    Status = "Completed",
                    ScoredAt = DateTime.UtcNow
                };
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Lỗi parse AI response: {ex.Message}");
            }
        }
        private string BuildScoringPrompt(string answerKey, string submission, string criteria, int maxScore)
        {
            return $@"Bạn là một giáo viên chuyên nghiệp chấm bài tự luận. 
                    Hãy chấm bài nộp của học sinh dựa trên đáp án chuẩn và tiêu chí chấm điểm.
                    bên trong ĐÁP ÁN CHUẨN có thể là tiêu chí của bài làm hoặc bài mẫu.

                    === ĐÁP ÁN CHUẨN ===
                    {answerKey}

                    === BÀI NỘP CỦA HỌC SINH ===
                    {submission}

                    === TIÊU CHÍ CHẤM ĐIỂM ===
                    {criteria}

                    === TỔNG ĐIỂM TỐI ĐA: {maxScore} điểm ===

                    YÊU CẦU CHẤM ĐIỂM:
                    1. So sánh nội dung bài nộp với đáp án chuẩn
                    2. Đánh giá mức độ chính xác, đầy đủ của câu trả lời
                    3. Xem xét cách trình bày, lập luận
                    4. Cho điểm công bằng và khách quan

                    Hãy trả lời theo định dạng JSON sau:
                    {{
                    ""score"": <số điểm từ 0 đến {maxScore}>,
                    ""percentage"": <phần trăm từ 0 đến 100>,
                    ""feedback"": ""Nhận xét tổng quan về bài làm của học sinh"",
                    ""strengths"": [
                        ""Điểm mạnh 1"",
                        ""Điểm mạnh 2""
                    ],
                    ""weaknesses"": [
                        ""Điểm yếu 1"",
                        ""Điểm yếu 2""
                    ],
                    ""improvement"": ""Gợi ý cải thiện cho học sinh""
                    }}

                    QUAN TRỌNG: Chỉ trả về JSON thuần túy, không có markdown code block (```), không có text thừa.";
        }
        private async Task<byte[]> DownloadFileFromUrlAsync(string fileUrl)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(fileUrl))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Lỗi khi tải file: {response.StatusCode}");
                    }
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Lỗi khi tải file từ URL '{fileUrl}': {ex.Message}");
            }
        }
        private string ExtractTextFromDocx(byte[] fileContent)
        {
            try
            {
                using (var memoryStream = new MemoryStream(fileContent))
                using (var document = WordprocessingDocument.Open(memoryStream, false))
                {
                    var body = document.MainDocumentPart?.Document?.Body;
                    if (body != null)
                    {
                        return body.InnerText;
                    }
                    return string.Empty;
                }
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Lỗi khi đọc file DOCX: {ex.Message}");
            }
        }
        private class OpenRouterResponse
        {
            [JsonPropertyName("choices")]
            public List<Choice> Choices { get; set; }
        }

        private class Choice
        {
            [JsonPropertyName("message")]
            public Message Message { get; set; }
        }

        private class Message
        {
            [JsonPropertyName("content")]
            public string Content { get; set; }
        }

        int GetInt(JsonElement e, string name, int def = 0)
        {
            if (!e.TryGetProperty(name, out var p)) return def;
            return p.ValueKind switch
            {
                JsonValueKind.Number => p.TryGetInt32(out var v) ? v : def,
                JsonValueKind.String => int.TryParse(p.GetString(), out var v2) ? v2 : def,
                _ => def
            };
        }
        double GetDouble(JsonElement e, string name, double def = 0)
        {
            if (!e.TryGetProperty(name, out var p)) return def;
            return p.ValueKind switch
            {
                JsonValueKind.Number => p.GetDouble(),
                JsonValueKind.String => double.TryParse(p.GetString(), out var v) ? v : def,
                _ => def
            };
        }
        string GetString(JsonElement e, string name) =>
            e.TryGetProperty(name, out var p) ? p.GetString() ?? "" : "";
    }

}