using ProbashiShield.Database.DBEntities.Common;
using System;

namespace ProbashiShield.Database.DBEntities
{
    public class CountryFeeLimit : BaseEntity
    {
        public long CountryId { get; set; }

        public decimal MaximumAllowedFee { get; set; }

        public string MaximumAllowedFeeDisplay =>
            MaximumAllowedFee.ToString("N2") ?? "0.00";

        public DateTime? EffectiveDate { get; set; }
    }
}