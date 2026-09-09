using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class OCRResultRepository : GenericRepository<OCRResult, MasterDbContext>, IOCRResultRepository
    {
        public OCRResultRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
