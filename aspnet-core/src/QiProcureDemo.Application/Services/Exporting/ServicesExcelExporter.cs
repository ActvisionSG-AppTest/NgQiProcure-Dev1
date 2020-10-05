using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Services.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Services.Exporting
{
    public class ServicesExcelExporter : NpoiExcelExporterBase, IServicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ServicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetServiceForViewDto> services)
        {
            return CreateExcelPackage(
                "Services.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Services"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Duration"),
                        L("IsApproved"),
                        L("IsActive"),
                        L("Remark"),
                        (L("Category")) + L("Name"),
                        (L("SysRef")) + L("RefCode")
                        );

                    AddObjects(
                        sheet, 2, services,
                        _ => _.Service.Name,
                        _ => _.Service.Description,
                        _ => _.Service.Duration,
                        _ => _.Service.IsApproved,
                        _ => _.Service.IsActive,
                        _ => _.Service.Remark,
                        _ => _.CategoryName,
                        _ => _.SysRefRefCode
                        );


                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
