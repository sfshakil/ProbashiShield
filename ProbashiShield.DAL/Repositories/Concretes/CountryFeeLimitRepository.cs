using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class CountryFeeLimitRepository : GenericRepository<CountryFeeLimit, MasterDbContext>, ICountryFeeLimitRepository
    {
        public CountryFeeLimitRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
