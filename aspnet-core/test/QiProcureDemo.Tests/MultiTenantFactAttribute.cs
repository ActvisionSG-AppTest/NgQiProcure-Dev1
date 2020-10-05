using Xunit;

namespace QiProcureDemo.Tests
{
    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        private readonly bool _multiTenancyEnabled = QiProcureDemoConsts.MultiTenancyEnabled;

        public MultiTenantFactAttribute()
        {
            if (!_multiTenancyEnabled)
            {
                Skip = "MultiTenancy is disabled.";
            }
        }
    }
}
