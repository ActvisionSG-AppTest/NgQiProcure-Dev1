using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.SysStatuses.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.SysStatuses.Exporting
{
    public class SysStatusesExcelExporter : NpoiExcelExporterBase, ISysStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SysStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSysStatusForViewDto> SysStatuses)
        {
            return CreateExcelPackage(
                "SysStatuses.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("SysStatuses"));
                    

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Description"),
                        (L("SysRef")) + L("TenantId")
                        );

                    AddObjects(
                        sheet, 2, SysStatuses,
                        _ => _.SysStatus.Code,
                        _ => _.SysStatus.Name,
                        _ => _.SysStatus.Description,
                        _ => _.SysRefTenantId
                        );

                    for (var i = 0; i < 4; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


                });
        }
    }
}
