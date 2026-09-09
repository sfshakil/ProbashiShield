using System;

namespace ProbashiShield.Domain.ViewModels.Admin
{
    public class AgencyViewModel
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; }
        public string AgencyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AgencyStatus { get; set; }
        public DateTime? ValidityDate { get; set; }
        public DateTime? LastSyncDate { get; set; }
    }
}
