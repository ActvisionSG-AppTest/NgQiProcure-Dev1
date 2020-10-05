using Abp.AspNetCore.Mvc.ViewComponents;

namespace QiProcureDemo.Web.Public.Views
{
    public abstract class QiProcureDemoViewComponent : AbpViewComponent
    {
        protected QiProcureDemoViewComponent()
        {
            LocalizationSourceName = QiProcureDemoConsts.LocalizationSourceName;
        }
    }
}