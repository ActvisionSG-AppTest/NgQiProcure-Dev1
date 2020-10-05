using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo.Startup
{
    [DependsOn(typeof(QiProcureDemoCoreModule))]
    public class QiProcureDemoGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}