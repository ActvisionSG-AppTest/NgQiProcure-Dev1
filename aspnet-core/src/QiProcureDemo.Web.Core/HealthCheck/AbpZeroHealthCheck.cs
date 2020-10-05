using Microsoft.Extensions.DependencyInjection;
using QiProcureDemo.HealthChecks;

namespace QiProcureDemo.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<QiProcureDemoDbContextHealthCheck>("Database Connection");
            builder.AddCheck<QiProcureDemoDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
