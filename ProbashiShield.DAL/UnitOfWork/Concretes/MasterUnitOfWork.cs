using Microsoft.EntityFrameworkCore.Storage;
using ProbashiShield.DAL.Repositories.Concretes;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Database.DBContexts;
using System.Threading.Tasks;

namespace ProbashiShield.DAL.UnitOfWork.Concretes
{
    public class MasterUnitOfWork : IMasterUnitOfWork
    {
        private readonly MasterDbContext _dataContext;
        private IDbContextTransaction _transaction;

        public MasterUnitOfWork(MasterDbContext dataContext)
        {
            _dataContext = dataContext;
            AgencyRepository = new AgencyRepository(_dataContext);
            AgencySyncLogRepository = new AgencySyncLogRepository(_dataContext);
            AIAnalysisLogRepository = new AIAnalysisLogRepository(_dataContext);
            CountryRepository = new CountryRepository(_dataContext);
            CountryFeeLimitRepository = new CountryFeeLimitRepository(_dataContext);
            DocumentRepository = new DocumentRepository(_dataContext);
            ErrorLogRepository = new ErrorLogRepository(_dataContext);
            JobCategoryRepository = new JobCategoryRepository(_dataContext);
            OCRResultRepository = new OCRResultRepository(_dataContext);
            RiskFindingRepository = new RiskFindingRepository(_dataContext);
            RiskRuleRepository = new RiskRuleRepository(_dataContext);
            SalaryReferenceRepository = new SalaryReferenceRepository(_dataContext);
            VerificationRequestRepository = new VerificationRequestRepository(_dataContext);
            VerificationResultRepository = new VerificationResultRepository(_dataContext);
        }

        public IAgencyRepository AgencyRepository { get; private set; }
        public IAgencySyncLogRepository AgencySyncLogRepository { get; private set; }
        public IAIAnalysisLogRepository AIAnalysisLogRepository { get; private set; }
        public ICountryRepository CountryRepository { get; private set; }
        public ICountryFeeLimitRepository CountryFeeLimitRepository { get; private set; }
        public IDocumentRepository DocumentRepository { get; private set; }
        public IErrorLogRepository ErrorLogRepository { get; private set; }
        public IJobCategoryRepository JobCategoryRepository { get; private set; }
        public IOCRResultRepository OCRResultRepository { get; private set; }
        public IRiskFindingRepository RiskFindingRepository { get; private set; }
        public IRiskRuleRepository RiskRuleRepository { get; private set; }
        public ISalaryReferenceRepository SalaryReferenceRepository { get; private set; }
        public IVerificationRequestRepository VerificationRequestRepository { get; private set; }
        public IVerificationResultRepository VerificationResultRepository { get; private set; }

        public async Task<int> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dataContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _dataContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }
    }
}
