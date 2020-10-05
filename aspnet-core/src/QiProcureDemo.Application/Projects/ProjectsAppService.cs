using QiProcureDemo.Accounts;
using QiProcureDemo.Teams;
using QiProcureDemo.SysStatuses;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Projects.Exporting;
using QiProcureDemo.Projects.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.Projects
{
	[AbpAuthorize(AppPermissions.Pages_Projects)]
    public class ProjectsAppService : QiProcureDemoAppServiceBase, IProjectsAppService
    {
		 private readonly IRepository<Project> _projectRepository;
		 private readonly IProjectsExcelExporter _projectsExcelExporter;
		 private readonly IRepository<Account,int> _lookup_accountRepository;
		 private readonly IRepository<Team,int> _lookup_teamRepository;
		 private readonly IRepository<SysStatus,int> _lookup_sysStatusRepository;
		 

		  public ProjectsAppService(IRepository<Project> projectRepository, IProjectsExcelExporter projectsExcelExporter , IRepository<Account, int> lookup_accountRepository, IRepository<Team, int> lookup_teamRepository, IRepository<SysStatus, int> lookup_sysStatusRepository) 
		  {
			_projectRepository = projectRepository;
			_projectsExcelExporter = projectsExcelExporter;
			_lookup_accountRepository = lookup_accountRepository;
		_lookup_teamRepository = lookup_teamRepository;
		_lookup_sysStatusRepository = lookup_sysStatusRepository;
		
		  }

		 public async Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input)
         {
			
			var filteredProjects = _projectRepository.GetAll()
						.Include( e => e.AccountFk)
						.Include( e => e.TeamFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(input.IsApproveFilter > -1,  e => (input.IsApproveFilter == 1 && e.IsApprove) || (input.IsApproveFilter == 0 && !e.IsApprove) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(input.PublishFilter > -1,  e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountFk != null && e.AccountFk.Name == input.AccountNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var pagedAndFilteredProjects = filteredProjects
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var projects = from o in pagedAndFilteredProjects
                         join o1 in _lookup_accountRepository.GetAll() on o.AccountId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_teamRepository.GetAll() on o.TeamId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProjectForViewDto() {
							Project = new ProjectDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                IsApprove = o.IsApprove,
                                IsActive = o.IsActive,
                                Publish = o.Publish,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	AccountName = s1 == null ? "" : s1.Name.ToString(),
                         	TeamName = s2 == null ? "" : s2.Name.ToString(),
                         	SysStatusName = s3 == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredProjects.CountAsync();

            return new PagedResultDto<GetProjectForViewDto>(
                totalCount,
                await projects.ToListAsync()
            );
         }
		 
		 public async Task<GetProjectForViewDto> GetProjectForView(int id)
         {
            var project = await _projectRepository.GetAsync(id);

            var output = new GetProjectForViewDto { Project = ObjectMapper.Map<ProjectDto>(project) };

		    if (output.Project.AccountId != null)
            {
                var _lookupAccount = await _lookup_accountRepository.FirstOrDefaultAsync((int)output.Project.AccountId);
                output.AccountName = _lookupAccount.Name.ToString();
            }

		    if (output.Project.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.Project.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

		    if (output.Project.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Project.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
		 public async Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input)
         {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProjectForEditOutput {Project = ObjectMapper.Map<CreateOrEditProjectDto>(project)};

		    if (output.Project.AccountId != null)
            {
                var _lookupAccount = await _lookup_accountRepository.FirstOrDefaultAsync((int)output.Project.AccountId);
                output.AccountName = _lookupAccount.Name.ToString();
            }

		    if (output.Project.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.Project.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

		    if (output.Project.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Project.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProjectDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Create)]
		 protected virtual async Task Create(CreateOrEditProjectDto input)
         {
            var project = ObjectMapper.Map<Project>(input);

			
			if (AbpSession.TenantId != null)
			{
				project.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _projectRepository.InsertAsync(project);
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
		 protected virtual async Task Update(CreateOrEditProjectDto input)
         {
            var project = await _projectRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, project);
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _projectRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input)
         {
			
			var filteredProjects = _projectRepository.GetAll()
						.Include( e => e.AccountFk)
						.Include( e => e.TeamFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(input.IsApproveFilter > -1,  e => (input.IsApproveFilter == 1 && e.IsApprove) || (input.IsApproveFilter == 0 && !e.IsApprove) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(input.PublishFilter > -1,  e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountFk != null && e.AccountFk.Name == input.AccountNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var query = (from o in filteredProjects
                         join o1 in _lookup_accountRepository.GetAll() on o.AccountId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_teamRepository.GetAll() on o.TeamId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProjectForViewDto() { 
							Project = new ProjectDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                IsApprove = o.IsApprove,
                                IsActive = o.IsActive,
                                Publish = o.Publish,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	AccountName = s1 == null ? "" : s1.Name.ToString(),
                         	TeamName = s2 == null ? "" : s2.Name.ToString(),
                         	SysStatusName = s3 == null ? "" : s3.Name.ToString()
						 });


            var projectListDtos = await query.ToListAsync();

            return _projectsExcelExporter.ExportToFile(projectListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectAccountLookupTableDto>> GetAllAccountForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_accountRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var accountList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectAccountLookupTableDto>();
			foreach(var account in accountList){
				lookupTableDtoList.Add(new ProjectAccountLookupTableDto
				{
					Id = account.Id,
					DisplayName = account.Name?.ToString()
				});
			}

            return new PagedResultDto<ProjectAccountLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_teamRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var teamList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectTeamLookupTableDto>();
			foreach(var team in teamList){
				lookupTableDtoList.Add(new ProjectTeamLookupTableDto
				{
					Id = team.Id,
					DisplayName = team.Name?.ToString()
				});
			}

            return new PagedResultDto<ProjectTeamLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectSysStatusLookupTableDto>();
			foreach(var sysStatus in sysStatusList){
				lookupTableDtoList.Add(new ProjectSysStatusLookupTableDto
				{
					Id = sysStatus.Id,
					DisplayName = sysStatus.Name?.ToString()
				});
			}

            return new PagedResultDto<ProjectSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}