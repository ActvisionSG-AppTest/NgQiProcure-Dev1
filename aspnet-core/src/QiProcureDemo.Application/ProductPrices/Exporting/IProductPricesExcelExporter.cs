using System.Collections.Generic;
using QiProcureDemo.ProductPrices.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ProductPrices.Exporting
{
    public interface IProductPricesExcelExporter
    {
        FileDto ExportToFile(List<GetProductPriceForViewDto> productPrices);
    }
}