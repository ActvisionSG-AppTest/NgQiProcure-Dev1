using System.Collections.Generic;
using MvvmHelpers;
using QiProcureDemo.Models.NavigationMenu;

namespace QiProcureDemo.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}