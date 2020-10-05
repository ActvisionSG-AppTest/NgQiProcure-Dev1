using Abp.Authorization;
using QiProcureDemo.Authorization.Roles;
using QiProcureDemo.Authorization.Users;

namespace QiProcureDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
