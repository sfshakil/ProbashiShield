using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Contracts
{
    public interface ICountryFeeLimitRepository : IGenericRepository<CountryFeeLimit, MasterDbContext>
    {
    }
}
