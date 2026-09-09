using System;

namespace ProbashiShield.Database.DBEntities
{
    public class VerificationResult
    {
        public long Id { get; set; }

        public long ResultId { get; set; }

        public string Verdict { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}