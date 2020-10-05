using System.Collections.Generic;
using QiProcureDemo.ProjectInstructions.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ProjectInstructions.Exporting
{
    public interface IProjectInstructionsExcelExporter
    {
        FileDto ExportToFile(List<GetProjectInstructionForViewDto> projectInstructions);
    }
}