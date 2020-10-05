using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Approvals.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Approvals.Exporting
{
    public class ApprovalsExcelExporter : NpoiExcelExporterBase, IApprovalsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApprovalsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApprovalForViewDto> approvals)
        {
            return CreateExcelPackage(
                "Approvals.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Approvals"));
                    

                    AddHeader(
                        sheet,
                        L("RankNo"),
                        L("Amount"),
                        (L("SysRef")) + L("TenantId"),
                        (L("Team")) + L("Name"),
                        (L("Project")) + L("Name"),
                        (L("Account")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("SysStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, approvals,
                        _ => _.Approval.RankNo,
                        _ => _.Approval.Amount,
                        _ => _.SysRefTenantId,
                        _ => _.TeamName,
                        _ => _.ProjectName,
                        _ => _.AccountName,
                        _ => _.UserName,
                        _ => _.SysStatusName
                        );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
