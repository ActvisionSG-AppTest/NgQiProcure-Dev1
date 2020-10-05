using System.Collections.Generic;
using QiProcureDemo.ServicePrices.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ServicePrices.Exporting
{
    public interface IServicePricesExcelExporter
    {
        FileDto ExportToFile(List<GetServicePriceForViewDto> servicePrices);
    }
}