using Abp.Events.Bus;

namespace QiProcureDemo.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}