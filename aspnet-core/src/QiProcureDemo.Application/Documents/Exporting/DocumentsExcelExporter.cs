using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Documents.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Documents.Exporting
{
    public class DocumentsExcelExporter : NpoiExcelExporterBase, IDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDocumentForViewDto> documents)
        {
            return CreateExcelPackage(
                "Documents.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Documents"));
                    

                    AddHeader(
                        sheet,
                        L("Url"),
                        L("Name"),
                        L("Description"),
                        (L("SysRef")) + L("TenantId"),
                        (L("Product")) + L("Name"),
                        (L("Service")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, documents,
                        _ => _.Document.Url,
                        _ => _.Document.Name,
                        _ => _.Document.Description,
                        _ => _.SysRefTenantId,
                        _ => _.ProductName,
                        _ => _.ServiceName
                        );

                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


                });
        }
    }
}
