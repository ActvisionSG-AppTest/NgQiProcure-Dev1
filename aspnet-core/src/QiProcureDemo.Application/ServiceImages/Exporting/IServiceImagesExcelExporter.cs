using System.Collections.Generic;
using QiProcureDemo.ServiceImages.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ServiceImages.Exporting
{
    public interface IServiceImagesExcelExporter
    {
        FileDto ExportToFile(List<GetServiceImageForViewDto> serviceImages);
    }
}