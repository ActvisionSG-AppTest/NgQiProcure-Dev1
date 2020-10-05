using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo
{
    public class QiProcureDemoCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoCoreSharedModule).GetAssembly());
        }
    }
}