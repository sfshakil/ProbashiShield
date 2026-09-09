using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class CountryRepository : GenericRepository<Country, MasterDbContext>, ICountryRepository
    {
        public CountryRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
