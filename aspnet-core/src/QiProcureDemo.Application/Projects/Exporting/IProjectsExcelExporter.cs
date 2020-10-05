using System.Collections.Generic;
using QiProcureDemo.Projects.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Projects.Exporting
{
    public interface IProjectsExcelExporter
    {
        FileDto ExportToFile(List<GetProjectForViewDto> projects);
    }
}