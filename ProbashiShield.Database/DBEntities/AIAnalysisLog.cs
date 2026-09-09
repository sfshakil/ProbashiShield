using System;

namespace ProbashiShield.Database.DBEntities
{
    public class AIAnalysisLog
    {
        public long Id { get; set; }

        public long ResultId { get; set; }

        public string? RiskLevel { get; set; }

        public string? PromptText { get; set; }

        public string? AIResponse { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}