using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class SalaryReferenceRepository : GenericRepository<SalaryReference, MasterDbContext>, ISalaryReferenceRepository
    {
        public SalaryReferenceRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
