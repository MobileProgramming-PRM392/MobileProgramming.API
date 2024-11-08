
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Quartz;
using MobileProgramming.Business.Quartz.PaymentScheduler;
using Serilog;
namespace MobileProgramming.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<ILogger>(Log.Logger);

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddQuartz(q =>
            {
                var checkJobKey = new JobKey("CheckTransactionStatusJob");
                q.AddJob<CheckTransactionStatusJob>(opts => opts.WithIdentity(checkJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(checkJobKey)
                    .WithIdentity("CheckTransactionStatusTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(1)
                        .RepeatForever()
                        .Build()));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }
    }
}
