using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo
{
    [DependsOn(typeof(QiProcureDemoXamarinSharedModule))]
    public class QiProcureDemoXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoXamarinAndroidModule).GetAssembly());
        }
    }
}