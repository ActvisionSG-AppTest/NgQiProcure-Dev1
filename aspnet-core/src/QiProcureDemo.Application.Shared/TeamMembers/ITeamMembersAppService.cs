using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.TeamMembers.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.TeamMembers
{
    public interface ITeamMembersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTeamMemberForViewDto>> GetAll(GetAllTeamMembersInput input);

        Task<GetTeamMemberForViewDto> GetTeamMemberForView(int id);

		Task<GetTeamMemberForEditOutput> GetTeamMemberForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTeamMemberDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTeamMembersToExcel(GetAllTeamMembersForExcelInput input);

		
		Task<PagedResultDto<TeamMemberTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TeamMemberUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TeamMemberSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TeamMemberSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);
		
    }
}