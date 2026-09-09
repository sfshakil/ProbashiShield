using ProbashiShield.Database.DBEntities.Common;

namespace ProbashiShield.Database.DBEntities
{
    public class Country : BaseEntity
    {
        public string CountryCode { get; set; } = string.Empty;

        public string CountryName { get; set; } = string.Empty;
    }
}