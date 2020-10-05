using System.Collections.Generic;
using QiProcureDemo.Authorization.Permissions.Dto;

namespace QiProcureDemo.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}