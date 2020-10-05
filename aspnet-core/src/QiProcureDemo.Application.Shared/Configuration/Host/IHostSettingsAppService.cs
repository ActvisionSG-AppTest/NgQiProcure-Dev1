using System.Threading.Tasks;
using Abp.Application.Services;
using QiProcureDemo.Configuration.Host.Dto;

namespace QiProcureDemo.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
