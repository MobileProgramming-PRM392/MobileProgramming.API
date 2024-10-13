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
using MobileProgramming.Data.Interfaces;


namespace MobileProgramming.Data.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSetting>(configuration.GetSection("Redis"));

            //Add DBcontext
            services.AddDbContext<SaleProductDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("local"),
                    //configuration.GetConnectionString("production"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(SaleProductDbContext).Assembly.FullName);
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
           
            //Add session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MobileProgramming.Session";
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //add distributed lock with redis in DI container
            services.AddSingleton<IDistributedLockProvider>(_ =>
            new RedisDistributedSynchronizationProvider(redisDatabase!.GetDatabase()));

            services.AddSingleton<IRedisCaching, RedisCaching>();
            services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<SaleProductDbContext>());
           

            services.AddScoped<IProductRepository, ProductRepository>();
            return services;


        }
    }
}
