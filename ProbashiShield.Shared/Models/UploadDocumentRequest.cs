using System.Collections.Generic;

namespace ProbashiShield.Shared.Models
{
    public class UploadDocumentRequest
    {
        public List<DocumentFile> Documents { get; set; }
    }

    public class DocumentFile
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileData { get; set; } // Base64
        public string DocumentType { get; set; } // Each file has its own type
    }
}
