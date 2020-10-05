using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using QiProcureDemo.Authorization;

namespace QiProcureDemo
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(QiProcureDemoApplicationSharedModule),
        typeof(QiProcureDemoCoreModule)
        )]
    public class QiProcureDemoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoApplicationModule).GetAssembly());
        }
    }
}