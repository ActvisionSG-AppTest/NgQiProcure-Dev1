using System;
using QiProcureDemo.Core;
using QiProcureDemo.Core.Dependency;
using QiProcureDemo.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QiProcureDemo.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}