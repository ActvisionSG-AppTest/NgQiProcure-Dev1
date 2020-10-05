using System.Collections.Generic;
using QiProcureDemo.ServiceCategories.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ServiceCategories.Exporting
{
    public interface IServiceCategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetServiceCategoryForViewDto> serviceCategories);
    }
}