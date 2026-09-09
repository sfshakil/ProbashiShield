using System;

namespace ProbashiShield.Database.DBEntities
{
    public class OCRResult
    {
        public long Id { get; set; }

        public long RequestId { get; set; }

        public string? AgencyName { get; set; }

        public string? LicenseNumber { get; set; }

        public string? DestinationCountry { get; set; }

        public decimal? Salary { get; set; }

        public string SalaryDisplay =>
            Salary?.ToString("N2") ?? "0.00";

        public string? SalaryCurrency { get; set; }

        public decimal? RecruitmentFee { get; set; }

        public string RecruitmentFeeDisplay =>
            RecruitmentFee?.ToString("N2") ?? "0.00";

        public string? RecruitmentFeeCurrency { get; set; }

        public string? JobTitle { get; set; }

        public decimal? OCRConfidence { get; set; }

        public string? FullExtractedText { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}