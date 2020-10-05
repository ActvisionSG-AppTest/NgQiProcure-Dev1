using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Emails.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Emails.Exporting
{
    public class EmailsExcelExporter : NpoiExcelExporterBase, IEmailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmailForViewDto> emails)
        {
            return CreateExcelPackage(
                "Emails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Emails"));
                    

                    AddHeader(
                        sheet,
                        L("ReferenceId"),
                        L("EmailFrom"),
                        L("EmailTo"),
                        L("EmailCC"),
                        L("EmailBCC"),
                        L("Subject"),
                        L("Body"),
                        L("RequestDate"),
                        L("SentDate"),
                        (L("SysRef")) + L("TenantId"),
                        (L("SysStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, emails,
                        _ => _.Email.ReferenceId,
                        _ => _.Email.EmailFrom,
                        _ => _.Email.EmailTo,
                        _ => _.Email.EmailCC,
                        _ => _.Email.EmailBCC,
                        _ => _.Email.Subject,
                        _ => _.Email.Body,
                        _ => _timeZoneConverter.Convert(_.Email.RequestDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Email.SentDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SysRefTenantId,
                        _ => _.SysStatusName
                        );

                    for (var i = 1; i <= emails.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }

                    for (var i = 0; i < 11; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }                   

                });
        }
    }
}
