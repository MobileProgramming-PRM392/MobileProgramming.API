namespace MobileProgramming.API
{
    public static class HealthChecksConfiguration
    {
        public static IServiceCollection ConfigureHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("Redis:HostName").Get<string>();
            var sqlserver = configuration.GetSection("ConnectionStrings:production").Get<string>();
            var elasticSearch = configuration.GetSection("ElasticSearch:Url").Get<string>();
            services.AddHealthChecks()
                    .AddSqlServer(sqlserver!);
                  


            return services;
        }
    }
}
