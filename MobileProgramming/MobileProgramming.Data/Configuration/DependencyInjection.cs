using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileProgramming.Data.Persistence;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Medallion.Threading;
using Medallion.Threading.Redis;
using MobileProgramming.Data.ExternalServices.Caching.Setting;
using MobileProgramming.Data.ExternalServices.Caching;
using MobileProgramming.Data.Interfaces.Common;
using MobileProgramming.Data.Repository;


namespace MobileProgramming.Data.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSetting>(configuration.GetSection("Redis"));

            //Add DBcontext
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("local"),
                    //configuration.GetConnectionString("production"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                options.UseLazyLoadingProxies();
            });

            //Add redis
            var redisConnection = configuration["Redis:HostName"];
            var redisDatabase = ConnectionMultiplexer.Connect($"{redisConnection},abortConnect=false");
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return redisDatabase;
            });

            services.AddStackExchangeRedisCache(options =>
            {
                //var redisPassword = configuration["Redis:Password"];
                //options.Configuration = $"{redisConnection},password={redisPassword}";
                options.Configuration = redisConnection;
            });
           
            services.AddDistributedMemoryCache();

            //add distributed lock with redis in DI container
            services.AddSingleton<IDistributedLockProvider>(_ =>
            new RedisDistributedSynchronizationProvider(redisDatabase!.GetDatabase()));

            services.AddSingleton<IRedisCaching, RedisCaching>();
            services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<ProductDAL>();
            return services;


        }
    }
}
