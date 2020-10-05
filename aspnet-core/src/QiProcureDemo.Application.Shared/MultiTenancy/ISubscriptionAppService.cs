using System.Threading.Tasks;
using Abp.Application.Services;

namespace QiProcureDemo.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
