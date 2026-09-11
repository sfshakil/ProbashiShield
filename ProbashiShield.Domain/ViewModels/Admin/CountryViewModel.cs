using System;

namespace ProbashiShield.Domain.ViewModels.Admin
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CountryFeeLimitViewModel
    {
        public long Id { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public decimal MaximumAllowedFee { get; set; }
        public DateTime EffectiveDate { get; set; } = DateTime.Today;
        public bool IsActive { get; set; } = true;
    }

    public class SalaryReferenceViewModel
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int JobCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
