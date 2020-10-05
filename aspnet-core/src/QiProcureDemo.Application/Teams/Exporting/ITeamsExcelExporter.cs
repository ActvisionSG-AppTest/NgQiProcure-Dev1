using System.Collections.Generic;
using QiProcureDemo.Teams.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Teams.Exporting
{
    public interface ITeamsExcelExporter
    {
        FileDto ExportToFile(List<GetTeamForViewDto> teams);
    }
}