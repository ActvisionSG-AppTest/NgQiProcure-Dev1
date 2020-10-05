using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo
{
    [DependsOn(typeof(QiProcureDemoXamarinSharedModule))]
    public class QiProcureDemoXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoXamarinIosModule).GetAssembly());
        }
    }
}