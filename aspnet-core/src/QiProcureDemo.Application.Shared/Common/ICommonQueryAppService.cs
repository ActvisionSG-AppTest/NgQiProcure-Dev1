using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization.Roles.Dto;
using QiProcureDemo.Common.Dto;
using QiProcureDemo.Common.Dto.Custom;
using QiProcureDemo.Editions.Dto;

namespace QiProcureDemo.Common
{
    public interface ICommonQueryAppService : IApplicationService
    {      

        ListResultDto<RoleListDto> GetRoles();

        RequestOutput GetIsUserAdmin(GeneralInput input);
    }
}