using System.Collections.Generic;
using QiProcureDemo.SysStatuses.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.SysStatuses.Exporting
{
    public interface ISysStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetSysStatusForViewDto> SysStatuses);
    }
}