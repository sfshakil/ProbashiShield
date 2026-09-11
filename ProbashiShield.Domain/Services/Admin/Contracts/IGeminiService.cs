using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts
{
    public interface IGeminiService
    {
        Task<OllamaVerificationResult> AnalyzeForFraud(string prompt);
        Task<string> TranslateEngToBng(string prompt);
        Task<bool> IsHealthyAsync();
    }
}
