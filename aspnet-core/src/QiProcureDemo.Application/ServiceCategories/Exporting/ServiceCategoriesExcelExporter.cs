using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ServiceCategories.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ServiceCategories.Exporting
{
    public class ServiceCategoriesExcelExporter : NpoiExcelExporterBase, IServiceCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ServiceCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetServiceCategoryForViewDto> serviceCategories)
        {
            return CreateExcelPackage(
                "ServiceCategories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ServiceCategories"));
                    

                    AddHeader(
                        sheet,
                        (L("Service")) + L("Name"),
                        (L("Category")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, serviceCategories,
                        _ => _.ServiceName,
                        _ => _.CategoryName
                        );

                    for (var i = 0; i < 2; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
