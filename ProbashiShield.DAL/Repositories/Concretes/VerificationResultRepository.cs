using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class VerificationResultRepository : GenericRepository<VerificationResult, MasterDbContext>, IVerificationResultRepository
    {
        public VerificationResultRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
