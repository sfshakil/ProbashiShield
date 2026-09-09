using ProbashiShield.Database.DBEntities.Common;
using System;

namespace ProbashiShield.Database.DBEntities
{
    public class Agency : BaseEntity
    {
        public string LicenseNumber { get; set; } = string.Empty;

        public string AgencyName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? AgencyStatus { get; set; }

        public DateTime? ValidityDate { get; set; }

        public DateTime? LastSyncDate { get; set; }
    }
}