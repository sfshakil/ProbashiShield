using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class AIAnalysisLogRepository : GenericRepository<AIAnalysisLog, MasterDbContext>, IAIAnalysisLogRepository
    {
        public AIAnalysisLogRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
