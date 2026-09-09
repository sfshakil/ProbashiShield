using ProbashiShield.Database.DBEntities.Common;

namespace ProbashiShield.Database.DBEntities
{
    public class SalaryReference : BaseEntity
    {
        public long CountryId { get; set; }

        public long JobCategoryId { get; set; }

        public decimal MinSalary { get; set; }

        public string MinSalaryDisplay =>
            MinSalary.ToString("N2") ?? "0.00";

        public decimal MaxSalary { get; set; }

        public string MaxSalaryDisplay =>
            MaxSalary.ToString("N2") ?? "0.00";

        public string Currency { get; set; } = string.Empty;
    }
}