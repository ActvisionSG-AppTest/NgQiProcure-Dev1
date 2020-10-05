using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization.Permissions.Dto;

namespace QiProcureDemo.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
