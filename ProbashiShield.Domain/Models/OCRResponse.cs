namespace ProbashiShield.Domain.Models
{
    public class OCRResponse
    {
        public string? AgencyName { get; set; }

        public string? LicenseNumber { get; set; }

        public string? DestinationCountry { get; set; }

        public decimal? Salary { get; set; }
        public string? SalaryCurrency { get; set; }

        public decimal? RecruitmentFee { get; set; }
        public string? RecruitmentFeeCurrency { get; set; }

        public string? JobTitle { get; set; }

        public decimal? OCRConfidence { get; set; }

        public string? FullExtractedText { get; set; }

    }
}
