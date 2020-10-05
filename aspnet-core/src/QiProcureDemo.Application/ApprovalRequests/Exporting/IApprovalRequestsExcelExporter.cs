using System.Collections.Generic;
using QiProcureDemo.ApprovalRequests.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ApprovalRequests.Exporting
{
    public interface IApprovalRequestsExcelExporter
    {
        FileDto ExportToFile(List<GetApprovalRequestForViewDto> approvalRequests);
    }
}