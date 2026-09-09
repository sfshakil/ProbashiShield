using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class ErrorLogRepository : GenericRepository<ErrorLog, MasterDbContext>, IErrorLogRepository
    {
        public ErrorLogRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
