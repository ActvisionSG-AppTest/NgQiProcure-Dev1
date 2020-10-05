using System.Collections.Generic;
using QiProcureDemo.Auditing.Dto;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
