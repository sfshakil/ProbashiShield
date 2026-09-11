using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts
{
    public interface IOllamaService
    {
        /// <summary>
        /// Analyzes OCR data for fraud patterns and anomalies using local LLM
        /// </summary>
        Task<OllamaVerificationResult> AnalyzeForFraud(string prompt);

        /// <summary>
        /// Checks if Ollama service is available and healthy
        /// </summary>
        Task<bool> IsHealthyAsync();

        Task<string> TranslateEngToBng(string prompt);
    }

    public class OllamaVerificationResult
    {
        public bool IsFraudDetected { get; set; }
        public decimal RiskScore { get; set; } // 0.0 to 1.0
        public string Analysis { get; set; }
        public string Recommendation { get; set; }
        public string prompt { get; set; }
        public string Summary { get; set; }
        public decimal ConfidenceInAssessment { get; set; }
    }
}
