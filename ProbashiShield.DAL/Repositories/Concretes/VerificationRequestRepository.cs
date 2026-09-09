using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class VerificationRequestRepository : GenericRepository<VerificationRequest, MasterDbContext>, IVerificationRequestRepository
    {
        public VerificationRequestRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
