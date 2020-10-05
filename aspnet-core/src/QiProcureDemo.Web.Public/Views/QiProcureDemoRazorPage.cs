using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace QiProcureDemo.Web.Public.Views
{
    public abstract class QiProcureDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected QiProcureDemoRazorPage()
        {
            LocalizationSourceName = QiProcureDemoConsts.LocalizationSourceName;
        }
    }
}
