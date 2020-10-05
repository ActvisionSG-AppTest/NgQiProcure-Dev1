using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ProductPrices.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ProductPrices.Exporting
{
    public class ProductPricesExcelExporter : NpoiExcelExporterBase, IProductPricesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductPricesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductPriceForViewDto> productPrices)
        {
            return CreateExcelPackage(
                "ProductPrices.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ProductPrices"));
                    

                    AddHeader(
                        sheet,
                        L("Price"),
                        L("validity"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productPrices,
                        _ => _.ProductPrice.Price,
                        _ => _timeZoneConverter.Convert(_.ProductPrice.validity, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ProductName
                        );

                    for (var i = 1; i <= productPrices.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }

                    for (var i = 0; i < 3; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


                });
        }
    }
}
