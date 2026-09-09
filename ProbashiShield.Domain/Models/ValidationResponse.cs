namespace ProbashiShield.Domain.Models
{
    public class ValidationResponse
    {
        public bool IsInformationMissing { get; set; } = false;
        public bool IsAgencyFound { get; set; } = true;
        public bool IsAgencyNameMissmatch { get; set; } = false;
        public bool IsAgencyActive { get; set; } = true;
        public bool IsDestinationCountryFound { get; set; } = true;
        public bool IsCountryFeeLimitFound { get; set; } = true;
        public bool? IsRecruitementFeeHigherThenCountryFeeLimit { get; set; }
        public bool? IsRecruitementFeeLowerThenCountryFeeLimit { get; set; }
        public bool IsJobCategoryFound { get; set; } = true;
        public bool IsSalaryReferenceFound { get; set; } = true;
        public bool? IsSalaryHigherThenSalaryReferenceFeeLimit { get; set; }
        public bool? IsSalaryLowerThenSalaryReferenceFeeLimit { get; set; }
    }
}
