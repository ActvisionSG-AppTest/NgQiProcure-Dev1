using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.SysRefs.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.SysRefs.Exporting
{
    public class SysRefsExcelExporter : NpoiExcelExporterBase, ISysRefsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SysRefsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSysRefForViewDto> sysRefs)
        {
            return CreateExcelPackage(
                "SysRefs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("SysRefs"));
                    

                    AddHeader(
                        sheet,
                        L("RefCode"),
                        L("Description"),
                        L("OrderNumber"),
                        (L("ReferenceType")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, sysRefs,
                        _ => _.SysRef.RefCode,
                        _ => _.SysRef.Description,
                        _ => _.SysRef.OrderNumber,
                        _ => _.ReferenceTypeName
                        );


                    for (var i = 0; i < 4; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
