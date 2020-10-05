using Abp.AspNetCore.Mvc.Views;

namespace QiProcureDemo.Web.Views
{
    public abstract class QiProcureDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected QiProcureDemoRazorPage()
        {
            LocalizationSourceName = QiProcureDemoConsts.LocalizationSourceName;
        }
    }
}
