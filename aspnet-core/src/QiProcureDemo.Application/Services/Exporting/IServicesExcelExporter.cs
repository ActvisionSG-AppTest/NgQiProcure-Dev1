using System.Collections.Generic;
using QiProcureDemo.Services.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Services.Exporting
{
    public interface IServicesExcelExporter
    {
        FileDto ExportToFile(List<GetServiceForViewDto> services);
    }
}