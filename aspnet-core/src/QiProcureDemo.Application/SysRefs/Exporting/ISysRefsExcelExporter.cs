using System.Collections.Generic;
using QiProcureDemo.SysRefs.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.SysRefs.Exporting
{
    public interface ISysRefsExcelExporter
    {
        FileDto ExportToFile(List<GetSysRefForViewDto> sysRefs);
    }
}