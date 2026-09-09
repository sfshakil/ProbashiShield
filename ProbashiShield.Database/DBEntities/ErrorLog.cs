using System;

namespace ProbashiShield.Database.DBEntities
{
    public class ErrorLog
    {
        public long Id { get; set; }

        public string? ErrorSource { get; set; }

        public string? ErrorMessage { get; set; }

        public string? StackTrace { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}