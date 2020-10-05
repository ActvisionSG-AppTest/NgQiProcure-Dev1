using System.Threading.Tasks;
using Abp.Application.Services;
using QiProcureDemo.Configuration.Tenants.Dto;

namespace QiProcureDemo.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
