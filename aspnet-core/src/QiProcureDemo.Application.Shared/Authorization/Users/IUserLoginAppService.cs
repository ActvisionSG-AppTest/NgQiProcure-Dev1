using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization.Users.Dto;

namespace QiProcureDemo.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
