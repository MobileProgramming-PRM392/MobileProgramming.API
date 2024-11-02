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
using Infrastructure.ExternalServices.Authentication.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.ExternalServices.Authentication;
using MobileProgramming.Data.ExternalServices.UploadFile;
using MobileProgramming.Data.ExternalServices.Payment.ZaloPay;


namespace MobileProgramming.Data.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSetting>(configuration.GetSection("Redis"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            //Add DBcontext
            services.AddDbContext<SaleProductDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    //configuration.GetConnectionString("local"),
                    configuration.GetConnectionString("production"),
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


            
            //Add JWTconfig
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration.GetSection("JWTSettings:Issuer").Get<string>(),
                    ValidAudience = configuration.GetSection("JWTSettings:Audience").Get<string>(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSettings:Securitykey").Get<string>())),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    //ClockSkew = TimeSpan.Zero
                };
            });

            //add distributed lock with redis in DI container
            services.AddSingleton<IDistributedLockProvider>(_ =>
            new RedisDistributedSynchronizationProvider(redisDatabase!.GetDatabase()));

            services.AddSingleton<IRedisCaching, RedisCaching>();
            services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<SaleProductDbContext>());
           

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IZaloPayService, ZaloPayService>();
            return services;


        }
    }
}
