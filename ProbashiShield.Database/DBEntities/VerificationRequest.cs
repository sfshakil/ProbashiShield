using System;

namespace ProbashiShield.Database.DBEntities
{
    public class VerificationRequest
    {
        public long Id { get; set; }

        public Guid RequestId { get; set; }

        public string? MobileNumber { get; set; }

        public string? DeviceId { get; set; }

        public DateTime RequestedAt { get; set; }
    }
}