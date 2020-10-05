using Abp.Modules;
using Abp.Reflection.Extensions;

namespace QiProcureDemo
{
    public class QiProcureDemoClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoClientModule).GetAssembly());
        }
    }
}
