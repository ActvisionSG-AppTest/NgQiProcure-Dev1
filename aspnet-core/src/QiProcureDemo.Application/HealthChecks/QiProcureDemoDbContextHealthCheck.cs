using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using QiProcureDemo.EntityFrameworkCore;

namespace QiProcureDemo.HealthChecks
{
    public class QiProcureDemoDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public QiProcureDemoDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("QiProcureDemoDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("QiProcureDemoDbContext could not connect to database"));
        }
    }
}
