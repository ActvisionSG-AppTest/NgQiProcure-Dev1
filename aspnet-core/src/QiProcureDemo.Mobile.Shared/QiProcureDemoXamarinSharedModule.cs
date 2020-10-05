using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo
{
    [DependsOn(typeof(QiProcureDemoClientModule), typeof(AbpAutoMapperModule))]
    public class QiProcureDemoXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoXamarinSharedModule).GetAssembly());
        }
    }
}