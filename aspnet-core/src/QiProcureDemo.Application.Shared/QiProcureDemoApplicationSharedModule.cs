using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo
{
    [DependsOn(typeof(QiProcureDemoCoreSharedModule))]
    public class QiProcureDemoApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoApplicationSharedModule).GetAssembly());
        }
    }
}