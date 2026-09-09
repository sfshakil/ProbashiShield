using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class RiskFindingRepository : GenericRepository<RiskFinding, MasterDbContext>, IRiskFindingRepository
    {
        public RiskFindingRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
