using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class AgencySyncLogRepository : GenericRepository<AgencySyncLog, MasterDbContext>, IAgencySyncLogRepository
    {
        public AgencySyncLogRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
