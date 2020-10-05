using System.Threading.Tasks;
using Abp.Application.Services;
using QiProcureDemo.Sessions.Dto;

namespace QiProcureDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
