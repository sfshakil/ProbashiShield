using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class AgencyRepository : GenericRepository<Agency, MasterDbContext>, IAgencyRepository
    {
        public AgencyRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
