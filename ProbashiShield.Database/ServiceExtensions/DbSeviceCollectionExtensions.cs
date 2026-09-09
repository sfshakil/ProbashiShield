using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProbashiShield.Database.Constants;
using ProbashiShield.Database.DBContexts;
using System;


namespace ProbashiShield.Database.ServiceExtensions
{
    public static class DbSeviceCollectionExtensions
    {
        public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.AppConfigs = configuration;

            int dbCommandTimeoutSeconds = configuration["DbCommandExecutionTimeoutSeconds"] != null ?
                                        Convert.ToInt32(configuration["DbCommandExecutionTimeoutSeconds"])
                                        : 30;

            var CS_ConnBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("BblMasterDbConnStr"));
            CS_ConnBuilder.UserID = CS_ConnBuilder.UserID;
            CS_ConnBuilder.Password = CS_ConnBuilder.Password;
            services.AddDbContext<MasterDbContext>(options => options.UseSqlServer(
                CS_ConnBuilder.ConnectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.CommandTimeout(dbCommandTimeoutSeconds);
                }), ServiceLifetime.Scoped);
        }

    }
}