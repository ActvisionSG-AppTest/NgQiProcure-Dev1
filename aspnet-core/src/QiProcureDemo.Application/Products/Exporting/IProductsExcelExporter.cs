using System.Collections.Generic;
using QiProcureDemo.Products.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Products.Exporting
{
    public interface IProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductForViewDto> products);
    }
}