using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ApprovalRequests.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ApprovalRequests.Exporting
{
    public class ApprovalRequestsExcelExporter : NpoiExcelExporterBase, IApprovalRequestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApprovalRequestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApprovalRequestForViewDto> approvalRequests)
        {
            return CreateExcelPackage(
                "ApprovalRequests.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ApprovalRequests"));
                    
                    AddHeader(
                        sheet,
                        L("ReferenceId"),
                        L("OrderNo"),
                        L("RankNo"),
                        L("Amount"),
                        L("Remark"),
                        (L("SysRef")) + L("TenantId"),
                        (L("SysStatus")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, approvalRequests,
                        _ => _.ApprovalRequest.ReferenceId,
                        _ => _.ApprovalRequest.OrderNo,
                        _ => _.ApprovalRequest.RankNo,
                        _ => _.ApprovalRequest.Amount,
                        _ => _.ApprovalRequest.Remark,
                        _ => _.SysRefTenantId,
                        _ => _.SysStatusName,
                        _ => _.UserName
                        );

                  
                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
