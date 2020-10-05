using QiProcureDemo.Projects;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ProjectInstructions.Exporting;
using QiProcureDemo.ProjectInstructions.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.ProjectInstructions
{
	[AbpAuthorize(AppPermissions.Pages_ProjectInstructions)]
    public class ProjectInstructionsAppService : QiProcureDemoAppServiceBase, IProjectInstructionsAppService
    {
		 private readonly IRepository<ProjectInstruction> _projectInstructionRepository;
		 private readonly IProjectInstructionsExcelExporter _projectInstructionsExcelExporter;
		 private readonly IRepository<Project,int> _lookup_projectRepository;
		 

		  public ProjectInstructionsAppService(IRepository<ProjectInstruction> projectInstructionRepository, IProjectInstructionsExcelExporter projectInstructionsExcelExporter , IRepository<Project, int> lookup_projectRepository) 
		  {
			_projectInstructionRepository = projectInstructionRepository;
			_projectInstructionsExcelExporter = projectInstructionsExcelExporter;
			_lookup_projectRepository = lookup_projectRepository;
		
		  }

		 public async Task<PagedResultDto<GetProjectInstructionForViewDto>> GetAll(GetAllProjectInstructionsInput input)
         {
			
			var filteredProjectInstructions = _projectInstructionRepository.GetAll()
						.Include( e => e.ProjectFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Instructions.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
						.WhereIf(input.MinInstructionNoFilter != null, e => e.InstructionNo >= input.MinInstructionNoFilter)
						.WhereIf(input.MaxInstructionNoFilter != null, e => e.InstructionNo <= input.MaxInstructionNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.InstructionsFilter),  e => e.Instructions == input.InstructionsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameFilter), e => e.ProjectFk != null && e.ProjectFk.Name == input.ProjectNameFilter);

			var pagedAndFilteredProjectInstructions = filteredProjectInstructions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var projectInstructions = from o in pagedAndFilteredProjectInstructions
                         join o1 in _lookup_projectRepository.GetAll() on o.ProjectId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProjectInstructionForViewDto() {
							ProjectInstruction = new ProjectInstructionDto
							{
                                InstructionNo = o.InstructionNo,
                                Instructions = o.Instructions,
                                Remarks = o.Remarks,
                                IsActive = o.IsActive,
                                Id = o.Id
							},
                         	ProjectName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProjectInstructions.CountAsync();

            return new PagedResultDto<GetProjectInstructionForViewDto>(
                totalCount,
                await projectInstructions.ToListAsync()
            );
         }
		 
		 public async Task<GetProjectInstructionForViewDto> GetProjectInstructionForView(int id)
         {
            var projectInstruction = await _projectInstructionRepository.GetAsync(id);

            var output = new GetProjectInstructionForViewDto { ProjectInstruction = ObjectMapper.Map<ProjectInstructionDto>(projectInstruction) };

		    if (output.ProjectInstruction.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.ProjectInstruction.ProjectId);
                output.ProjectName = _lookupProject.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ProjectInstructions_Edit)]
		 public async Task<GetProjectInstructionForEditOutput> GetProjectInstructionForEdit(EntityDto input)
         {
            var projectInstruction = await _projectInstructionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProjectInstructionForEditOutput {ProjectInstruction = ObjectMapper.Map<CreateOrEditProjectInstructionDto>(projectInstruction)};

		    if (output.ProjectInstruction.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.ProjectInstruction.ProjectId);
                output.ProjectName = _lookupProject.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProjectInstructionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectInstructions_Create)]
		 protected virtual async Task Create(CreateOrEditProjectInstructionDto input)
         {
            var projectInstruction = ObjectMapper.Map<ProjectInstruction>(input);

			
			if (AbpSession.TenantId != null)
			{
				projectInstruction.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _projectInstructionRepository.InsertAsync(projectInstruction);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectInstructions_Edit)]
		 protected virtual async Task Update(CreateOrEditProjectInstructionDto input)
         {
            var projectInstruction = await _projectInstructionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, projectInstruction);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectInstructions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _projectInstructionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProjectInstructionsToExcel(GetAllProjectInstructionsForExcelInput input)
         {
			
			var filteredProjectInstructions = _projectInstructionRepository.GetAll()
						.Include( e => e.ProjectFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Instructions.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
						.WhereIf(input.MinInstructionNoFilter != null, e => e.InstructionNo >= input.MinInstructionNoFilter)
						.WhereIf(input.MaxInstructionNoFilter != null, e => e.InstructionNo <= input.MaxInstructionNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.InstructionsFilter),  e => e.Instructions == input.InstructionsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameFilter), e => e.ProjectFk != null && e.ProjectFk.Name == input.ProjectNameFilter);

			var query = (from o in filteredProjectInstructions
                         join o1 in _lookup_projectRepository.GetAll() on o.ProjectId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProjectInstructionForViewDto() { 
							ProjectInstruction = new ProjectInstructionDto
							{
                                InstructionNo = o.InstructionNo,
                                Instructions = o.Instructions,
                                Remarks = o.Remarks,
                                IsActive = o.IsActive,
                                Id = o.Id
							},
                         	ProjectName = s1 == null ? "" : s1.Name.ToString()
						 });


            var projectInstructionListDtos = await query.ToListAsync();

            return _projectInstructionsExcelExporter.ExportToFile(projectInstructionListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ProjectInstructions)]
         public async Task<PagedResultDto<ProjectInstructionProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_projectRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var projectList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectInstructionProjectLookupTableDto>();
			foreach(var project in projectList){
				lookupTableDtoList.Add(new ProjectInstructionProjectLookupTableDto
				{
					Id = project.Id,
					DisplayName = project.Name?.ToString()
				});
			}

            return new PagedResultDto<ProjectInstructionProjectLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}