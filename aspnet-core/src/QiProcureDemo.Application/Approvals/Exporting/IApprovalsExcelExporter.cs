using System.Collections.Generic;
using QiProcureDemo.Approvals.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Approvals.Exporting
{
    public interface IApprovalsExcelExporter
    {
        FileDto ExportToFile(List<GetApprovalForViewDto> approvals);
    }
}