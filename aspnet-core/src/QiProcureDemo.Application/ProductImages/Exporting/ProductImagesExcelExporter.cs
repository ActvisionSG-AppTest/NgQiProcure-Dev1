using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ProductImages.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ProductImages.Exporting
{
    public class ProductImagesExcelExporter : NpoiExcelExporterBase, IProductImagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductImagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductImageForViewDto> productImages)
        {
            return CreateExcelPackage(
                "ProductImages.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ProductImages"));
                    

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("Url"),
                        L("IsMain"),
                        L("IsApproved"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productImages,
                        _ => _.ProductImage.Description,
                        _ => _.ProductImage.Url,
                        _ => _.ProductImage.IsMain,
                        _ => _.ProductImage.IsApproved,
                        _ => _.ProductName
                        );

                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
