using System.Threading.Tasks;
using Abp.Application.Services;
using QiProcureDemo.Install.Dto;

namespace QiProcureDemo.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}