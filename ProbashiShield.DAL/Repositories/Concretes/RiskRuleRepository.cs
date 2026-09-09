using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class RiskRuleRepository : GenericRepository<RiskRule, MasterDbContext>, IRiskRuleRepository
    {
        public RiskRuleRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
