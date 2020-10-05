using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using QiProcureDemo.Configure;
using QiProcureDemo.Startup;
using QiProcureDemo.Test.Base;

namespace QiProcureDemo.GraphQL.Tests
{
    [DependsOn(
        typeof(QiProcureDemoGraphQLModule),
        typeof(QiProcureDemoTestBaseModule))]
    public class QiProcureDemoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoGraphQLTestModule).GetAssembly());
        }
    }
}