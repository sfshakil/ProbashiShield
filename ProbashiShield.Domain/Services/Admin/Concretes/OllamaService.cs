using ProbashiShield.Domain.Services.Admin.Contracts;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ollamaBaseUrl;
        private readonly string _modelName;
        private readonly string _translationModelName;

        public OllamaService(HttpClient httpClient,
            string ollamaBaseUrl = "http://localhost:11434",
            string modelName = "deepseek-r1",
            string translationModelName = "qwen2.5:7b-instruct")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _ollamaBaseUrl = ollamaBaseUrl;
            _modelName = modelName;
            _translationModelName = translationModelName;
        }

        public async Task<OllamaVerificationResult> AnalyzeForFraud(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    model = _modelName,
                    prompt = prompt,
                    stream = false,
                    format = "json",
                    think = false,
                    options = new
                    {
                        temperature = 0.3,
                        num_predict = 400
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_ollamaBaseUrl}/api/generate", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("response", out var responseText))
                {
                    return ParseFraudAnalysis(responseText.GetString());
                }

                return new OllamaVerificationResult
                {
                    IsFraudDetected = false,
                    RiskScore = 0.0m,
                    Analysis = "Unable to parse response",
                    Recommendation = "Review data manually"
                };
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Ollama service unavailable at {_ollamaBaseUrl}. Ensure Ollama is running locally. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error communicating with Ollama: {ex.Message}", ex);
            }
        }

        public async Task<string> TranslateEngToBng(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    model = _translationModelName,
                    messages = new[]
                    {
                        new { role = "system", content =
                            "You are a professional English-to-Bangla translation engine. " +
                            "Translate the given English text into Bangla (Bengali script, as used in Bangladesh). " +
                            "Output ONLY the Bangla translation. " +
                            "Never use Chinese, English, or any other script. " +
                            "No explanation, no quotes, no JSON, no <think> tags." },
                        new { role = "user", content = prompt }
                    },
                    stream = false,
                    options = new
                    {
                        temperature = 0.1,
                        num_predict = 1000
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_ollamaBaseUrl}/api/chat", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                var raw = root.TryGetProperty("message", out var msg) && msg.TryGetProperty("content", out var c)
                    ? c.GetString() ?? string.Empty
                    : string.Empty;

                var cleaned = System.Text.RegularExpressions.Regex.Replace(
                    raw, "<think>.*?</think>", "", System.Text.RegularExpressions.RegexOptions.Singleline).Trim();
                cleaned = cleaned.Trim('"', '`', '\n', ' ');

                cleaned = System.Text.RegularExpressions.Regex.Replace(
                    cleaned, @"[^\u0980-\u09FF0-9\s.,%()\-–।]", "");

                return cleaned.Trim();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Ollama service unavailable at {_ollamaBaseUrl}. Ensure Ollama is running locally. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error communicating with Ollama: {ex.Message}", ex);
            }
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

            // deepseek-r1 may still wrap output in <think>...</think> even with format=json.
            var cleaned = System.Text.RegularExpressions.Regex.Replace(
                analysisText, "<think>.*?</think>", "", System.Text.RegularExpressions.RegexOptions.Singleline).Trim();

            try
            {
                using var doc = JsonDocument.Parse(cleaned);
                var root = doc.RootElement;

                var fraudDetected = root.TryGetProperty("fraud_detected", out var fd) && fd.GetBoolean();
                var riskScore = root.TryGetProperty("risk_score", out var rs) ? rs.GetDecimal() : 0.0m;
                var confidence = root.TryGetProperty("confidence_in_assessment", out var conf) ? conf.GetDecimal() : 0.0m;
                result.ConfidenceInAssessment = confidence;
                var concerns = root.TryGetProperty("concerns", out var c) && c.ValueKind == JsonValueKind.Array
                    ? string.Join("; ", c.EnumerateArray().Select(x => x.GetString()))
                    : null;

                result.IsFraudDetected = fraudDetected;
                result.RiskScore = riskScore;
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
                // Model didn't return valid JSON — fall back to the old keyword heuristic
                // rather than silently returning "legitimate".
                var lowerText = cleaned.ToLower();

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

        public async Task<bool> IsHealthyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_ollamaBaseUrl}/api/tags");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
