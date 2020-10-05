using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Projects.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.Projects
{
    public interface IProjectsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input);

        Task<GetProjectForViewDto> GetProjectForView(int id);

		Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProjectDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input);

		
		Task<PagedResultDto<ProjectAccountLookupTableDto>> GetAllAccountForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProjectTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProjectSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);
		
    }
}