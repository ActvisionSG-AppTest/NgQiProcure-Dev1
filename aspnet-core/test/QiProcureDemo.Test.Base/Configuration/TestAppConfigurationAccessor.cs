using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using QiProcureDemo.Configuration;

namespace QiProcureDemo.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(QiProcureDemoTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
