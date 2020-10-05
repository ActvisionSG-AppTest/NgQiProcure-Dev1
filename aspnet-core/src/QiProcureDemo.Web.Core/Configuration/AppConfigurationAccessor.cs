using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using QiProcureDemo.Configuration;

namespace QiProcureDemo.Web.Configuration
{
    public class AppConfigurationAccessor: IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public AppConfigurationAccessor(IWebHostEnvironment env)
        {
            Configuration = env.GetAppConfiguration();
        }
    }
}
