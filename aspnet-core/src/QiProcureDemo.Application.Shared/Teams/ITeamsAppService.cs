using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Teams.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.Teams
{
    public interface ITeamsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTeamForViewDto>> GetAll(GetAllTeamsInput input);

        Task<GetTeamForViewDto> GetTeamForView(int id);

		Task<GetTeamForEditOutput> GetTeamForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTeamDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTeamsToExcel(GetAllTeamsForExcelInput input);

		
		Task<PagedResultDto<TeamSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TeamReferenceTypeLookupTableDto>> GetAllReferenceTypeForLookupTable(GetAllForLookupTableInput input);
		
    }
}