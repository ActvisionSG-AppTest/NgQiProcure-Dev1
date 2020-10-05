using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ProjectInstructions.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.ProjectInstructions
{
    public interface IProjectInstructionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProjectInstructionForViewDto>> GetAll(GetAllProjectInstructionsInput input);

        Task<GetProjectInstructionForViewDto> GetProjectInstructionForView(int id);

		Task<GetProjectInstructionForEditOutput> GetProjectInstructionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProjectInstructionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProjectInstructionsToExcel(GetAllProjectInstructionsForExcelInput input);

		
		Task<PagedResultDto<ProjectInstructionProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input);
		
    }
}