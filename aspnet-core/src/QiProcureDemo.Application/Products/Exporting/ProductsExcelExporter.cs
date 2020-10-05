using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Products.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Products.Exporting
{
    public class ProductsExcelExporter : NpoiExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return CreateExcelPackage(
                "Products.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Products"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Stock"),
                        L("Uom"),
                        L("IsApproved"),
                        L("IsActive"),
                        L("Remark"),
                        (L("Category")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, products,
                        _ => _.Product.Name,
                        _ => _.Product.Description,
                        _ => _.Product.Stock,
                        _ => _.Product.Uom,
                        _ => _.Product.IsApproved,
                        _ => _.Product.IsActive,
                        _ => _.Product.Remark,
                        _ => _.CategoryName
                        );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
