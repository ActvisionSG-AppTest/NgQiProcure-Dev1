using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Categories.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Categories.Exporting
{
    public class CategoriesExcelExporter : NpoiExcelExporterBase, ICategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCategoryForViewDto> categories)
        {
            return CreateExcelPackage(
                "Categories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Categories"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("IsApproved"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, categories,
                        _ => _.Category.Name,
                        _ => _.Category.Description,
                        _ => _.Category.IsApproved,
                        _ => _.Category.IsActive
                        );

                    for (var i = 0; i < 4; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
