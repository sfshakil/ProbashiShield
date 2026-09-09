using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProbashiShield.DAL.UnitOfWork.Concretes;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Database.DBContexts;

namespace ProbashiShield.DAL.ServiceExtensions
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BblMasterDbConnStr");

            services.AddDbContext<MasterDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IMasterUnitOfWork, MasterUnitOfWork>();
        }
    }
}
