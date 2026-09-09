using Microsoft.Extensions.DependencyInjection;
using ProbashiShield.Shared.Hashing.Concretes;
using ProbashiShield.Shared.Hashing.Contracts;
using ProbashiShield.Shared.MemCache;

namespace ProbashiShield.Shared.ServiceExtensions
{
    public static class SharedServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddTransient<ICipher, ShaCipher>();
            services.AddTransient<IHashManager, BCryptHashing>();
            services.AddTransient<ICacheService, MemCacheService>();
            return services;
        }
    }
}
