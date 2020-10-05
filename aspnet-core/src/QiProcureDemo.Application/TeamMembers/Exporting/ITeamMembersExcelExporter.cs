using System.Collections.Generic;
using QiProcureDemo.TeamMembers.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.TeamMembers.Exporting
{
    public interface ITeamMembersExcelExporter
    {
        FileDto ExportToFile(List<GetTeamMemberForViewDto> teamMembers);
    }
}