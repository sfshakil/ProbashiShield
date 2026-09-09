using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class JobCategoryRepository : GenericRepository<JobCategory, MasterDbContext>, IJobCategoryRepository
    {
        public JobCategoryRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
