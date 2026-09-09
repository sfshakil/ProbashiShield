using System;

namespace ProbashiShield.Database.DBEntities
{
    public class RiskFinding
    {
        public long Id { get; set; }

        public long VerificationResultId { get; set; }

        public string RuleCode { get; set; } = string.Empty;

        public string? FindingText { get; set; }

        public int Severity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}