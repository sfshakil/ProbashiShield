using System;

namespace ProbashiShield.Database.DBEntities
{
    public class Document
    {
        public long Id { get; set; }

        public long? VerificationRequestId { get; set; }

        public string DocumentType { get; set; } = string.Empty;

        public string? OriginalFileName { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public long? FileSize { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}