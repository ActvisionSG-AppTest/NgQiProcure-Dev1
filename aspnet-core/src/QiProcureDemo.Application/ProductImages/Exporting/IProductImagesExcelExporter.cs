using System.Collections.Generic;
using QiProcureDemo.ProductImages.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ProductImages.Exporting
{
    public interface IProductImagesExcelExporter
    {
        FileDto ExportToFile(List<GetProductImageForViewDto> productImages);
    }
}