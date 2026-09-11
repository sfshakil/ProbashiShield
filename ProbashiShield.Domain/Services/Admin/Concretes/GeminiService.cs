using Microsoft.Extensions.Configuration;
using ProbashiShield.Domain.Services.Admin.Contracts;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = config["GoogleAI:ApiKey"] ?? throw new InvalidOperationException("GoogleAI:ApiKey not configured");
            _model = config["GoogleAI:Model"] ?? "gemini-3.6-flash";
        }

        public async Task<OllamaVerificationResult> AnalyzeForFraud(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[] { new { parts = new[] { new { text = prompt } } } },
                    generationConfig = new
                    {
                        temperature = 0.3,
                        maxOutputTokens = 2048,
                        responseMimeType = "application/json",
                        responseSchema = new
                        {
                            type = "OBJECT",
                            properties = new
                            {
                                fraud_detected = new { type = "BOOLEAN" },
                                risk_score = new { type = "NUMBER" },
                                confidence_in_assessment = new { type = "NUMBER" },
                                concerns = new { type = "ARRAY", items = new { type = "STRING" } },
                                summary = new { type = "STRING", description = "One or two sentence plain-English explanation of the verdict" }
                            },
                            required = new[] { "fraud_detected", "risk_score", "confidence_in_assessment", "concerns", "summary" }
                        }
                    }
                };

                var url = $"{BaseUrl}/{_model}:generateContent?key={_apiKey}";
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Gemini API error {(int)response.StatusCode}: {errorBody}");
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                var text = ExtractText(responseBody);

                return ParseFraudAnalysis(text);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Gemini API unavailable. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error communicating with Gemini: {ex.Message}", ex);
            }
        }

        public async Task<string> TranslateEngToBng(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[] { new { parts = new[] { new { text = prompt } } } },
                    generationConfig = new
                    {
                        temperature = 0.1,
                        maxOutputTokens = 1024
                    }
                };

                var url = $"{BaseUrl}/{_model}:generateContent?key={_apiKey}";
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var raw = ExtractText(responseBody);

                var cleaned = raw.Trim('"', '`', '\n', ' ');
                cleaned = System.Text.RegularExpressions.Regex.Replace(
                    cleaned, @"[^\u0980-\u09FF0-9\s.,%()\-–।]", "");

                return cleaned.Trim();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Gemini API unavailable. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error communicating with Gemini: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsHealthyAsync()
        {
            try
            {
                var url = $"{BaseUrl}?key={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private string ExtractText(string responseBody)
        {
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            var text = root.GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(
                text, "<think>.*?</think>", "", System.Text.RegularExpressions.RegexOptions.Singleline).Trim();
        }

        private OllamaVerificationResult ParseFraudAnalysis(string analysisText)
        {
            var result = new OllamaVerificationResult
            {
                Analysis = analysisText,
                IsFraudDetected = false,
                RiskScore = 0.0m,
                Recommendation = "Data appears legitimate"
            };

            if (string.IsNullOrWhiteSpace(analysisText))
                return result;

            try
            {
                using var doc = JsonDocument.Parse(analysisText);
                var root = doc.RootElement;
                var summary = root.TryGetProperty("summary", out var s) ? s.GetString() : null;
                var fraudDetected = root.TryGetProperty("fraud_detected", out var fd) && fd.GetBoolean();
                var riskScore = root.TryGetProperty("risk_score", out var rs) ? rs.GetDecimal() : 0.0m;
                var confidence = root.TryGetProperty("confidence_in_assessment", out var conf) ? conf.GetDecimal() : 0.0m;
                var concerns = root.TryGetProperty("concerns", out var c) && c.ValueKind == JsonValueKind.Array
                    ? string.Join("; ", c.EnumerateArray().Select(x => x.GetString()))
                    : null;

                result.Summary = summary;
                result.IsFraudDetected = fraudDetected;
                result.RiskScore = riskScore;
                result.ConfidenceInAssessment = confidence;
                result.Recommendation = riskScore switch
                {
                    > 0.6m => $"High suspicion detected - manual review required. {concerns}",
                    >= 0.3m => $"Moderate concern - additional verification suggested. {concerns}",
                    _ => $"Data appears clean - proceed with confidence. {concerns}"
                };
                return result;
            }
            catch (JsonException)
            {
                var lowerText = analysisText.ToLower();

                if (lowerText.Contains("suspicious") || lowerText.Contains("fraud") || lowerText.Contains("anomaly"))
                {
                    result.IsFraudDetected = true;
                    result.RiskScore = 0.7m;
                    result.Recommendation = "High suspicion detected - manual review required";
                }
                else if (lowerText.Contains("unusual") || lowerText.Contains("concern") || lowerText.Contains("inconsistent"))
                {
                    result.RiskScore = 0.5m;
                    result.Recommendation = "Moderate concern - additional verification suggested";
                }

                return result;
            }
        }
    }
}