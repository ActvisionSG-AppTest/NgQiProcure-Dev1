using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ReferenceTypes.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ReferenceTypes.Exporting
{
    public class ReferenceTypesExcelExporter : NpoiExcelExporterBase, IReferenceTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReferenceTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReferenceTypeForViewDto> referenceTypes)
        {
            return CreateExcelPackage(
                "ReferenceTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ReferenceTypes"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("ReferenceTypeCode"),
                        L("ReferenceTypeGroup")
                        );

                    AddObjects(
                        sheet, 2, referenceTypes,
                        _ => _.ReferenceType.Name,
                        _ => _.ReferenceType.ReferenceTypeCode,
                        _ => _.ReferenceType.ReferenceTypeGroup
                        );

                    for (var i = 0; i < 3; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


                });
        }
    }
}
