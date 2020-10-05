using Abp.Application.Services;
using QiProcureDemo.Dto;
using QiProcureDemo.Logging.Dto;

namespace QiProcureDemo.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
