using ProbashiShield.DAL.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace ProbashiShield.DAL.UnitOfWork.Contracts
{
    public interface IMasterUnitOfWork : IDisposable
    {
        IAgencyRepository AgencyRepository { get; }
        IAgencySyncLogRepository AgencySyncLogRepository { get; }
        IAIAnalysisLogRepository AIAnalysisLogRepository { get; }
        ICountryRepository CountryRepository { get; }
        ICountryFeeLimitRepository CountryFeeLimitRepository { get; }
        IDocumentRepository DocumentRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        IJobCategoryRepository JobCategoryRepository { get; }
        IOCRResultRepository OCRResultRepository { get; }
        IRiskFindingRepository RiskFindingRepository { get; }
        IRiskRuleRepository RiskRuleRepository { get; }
        ISalaryReferenceRepository SalaryReferenceRepository { get; }
        IVerificationRequestRepository VerificationRequestRepository { get; }
        IVerificationResultRepository VerificationResultRepository { get; }

        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
