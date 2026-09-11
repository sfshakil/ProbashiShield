using ProbashiShield.Database.DBEntities;
using ProbashiShield.Domain.Models;
using ProbashiShield.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts
{
    public interface IDocumentsService
    {
        Task<long> UploadDocuments(VerificationRequest verificationRequest, List<DocumentFile> documents);
        Task<bool> OCRAnalysis(long requestId);
        Task<VerdictResult> ValidationAndAIAnalysis(long requestId);
    }
}
