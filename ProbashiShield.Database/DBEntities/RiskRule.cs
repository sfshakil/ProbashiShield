using ProbashiShield.Database.DBEntities.Common;

namespace ProbashiShield.Database.DBEntities
{
    public class RiskRule : BaseEntity
    {
        public string RuleCode { get; set; } = string.Empty;

        public string RuleName { get; set; } = string.Empty;

        public decimal Weight { get; set; }

        public string? RuleDescription { get; set; }
    }
}