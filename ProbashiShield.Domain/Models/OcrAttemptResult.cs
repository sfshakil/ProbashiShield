namespace ProbashiShield.Domain.Models
{
    public class OcrAttemptResult
    {
        public string Text { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
    }
}
