using System.Collections.Generic;
using QiProcureDemo.Categories.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Categories.Exporting
{
    public interface ICategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetCategoryForViewDto> categories);
    }
}