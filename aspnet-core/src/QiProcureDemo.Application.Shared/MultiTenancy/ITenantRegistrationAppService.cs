using System.Threading.Tasks;
using Abp.Application.Services;
using QiProcureDemo.Editions.Dto;
using QiProcureDemo.MultiTenancy.Dto;

namespace QiProcureDemo.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}