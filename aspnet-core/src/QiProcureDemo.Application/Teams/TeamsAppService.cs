using QiProcureDemo.SysStatuses;
using QiProcureDemo.ReferenceTypes;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Teams.Exporting;
using QiProcureDemo.Teams.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.Teams
{
	[AbpAuthorize(AppPermissions.Pages_Teams)]
    public class TeamsAppService : QiProcureDemoAppServiceBase, ITeamsAppService
    {
		 private readonly IRepository<Team> _teamRepository;
		 private readonly ITeamsExcelExporter _teamsExcelExporter;
		 private readonly IRepository<SysStatus,int> _lookup_sysStatusRepository;
		 private readonly IRepository<ReferenceType,int> _lookup_referenceTypeRepository;
		 

		  public TeamsAppService(IRepository<Team> teamRepository, ITeamsExcelExporter teamsExcelExporter , IRepository<SysStatus, int> lookup_sysStatusRepository, IRepository<ReferenceType, int> lookup_referenceTypeRepository) 
		  {
			_teamRepository = teamRepository;
			_teamsExcelExporter = teamsExcelExporter;
			_lookup_sysStatusRepository = lookup_sysStatusRepository;
		_lookup_referenceTypeRepository = lookup_referenceTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetTeamForViewDto>> GetAll(GetAllTeamsInput input)
         {
			
			var filteredTeams = _teamRepository.GetAll()
						.Include( e => e.SysStatusFk)
						.Include( e => e.ReferenceTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeNameFilter), e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == input.ReferenceTypeNameFilter);

			var pagedAndFilteredTeams = filteredTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var teams = from o in pagedAndFilteredTeams
                         join o1 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_referenceTypeRepository.GetAll() on o.ReferenceTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTeamForViewDto() {
							Team = new TeamDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	SysStatusName = s1 == null ? "" : s1.Name.ToString(),
                         	ReferenceTypeName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredTeams.CountAsync();

            return new PagedResultDto<GetTeamForViewDto>(
                totalCount,
                await teams.ToListAsync()
            );
         }
		 
		 public async Task<GetTeamForViewDto> GetTeamForView(int id)
         {
            var team = await _teamRepository.GetAsync(id);

            var output = new GetTeamForViewDto { Team = ObjectMapper.Map<TeamDto>(team) };

		    if (output.Team.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Team.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }

		    if (output.Team.ReferenceTypeId != null)
            {
                var _lookupReferenceType = await _lookup_referenceTypeRepository.FirstOrDefaultAsync((int)output.Team.ReferenceTypeId);
                output.ReferenceTypeName = _lookupReferenceType.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Teams_Edit)]
		 public async Task<GetTeamForEditOutput> GetTeamForEdit(EntityDto input)
         {
            var team = await _teamRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTeamForEditOutput {Team = ObjectMapper.Map<CreateOrEditTeamDto>(team)};

		    if (output.Team.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Team.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }

		    if (output.Team.ReferenceTypeId != null)
            {
                var _lookupReferenceType = await _lookup_referenceTypeRepository.FirstOrDefaultAsync((int)output.Team.ReferenceTypeId);
                output.ReferenceTypeName = _lookupReferenceType.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTeamDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Teams_Create)]
		 protected virtual async Task Create(CreateOrEditTeamDto input)
         {
            var team = ObjectMapper.Map<Team>(input);

			
			if (AbpSession.TenantId != null)
			{
				team.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _teamRepository.InsertAsync(team);
         }

		 [AbpAuthorize(AppPermissions.Pages_Teams_Edit)]
		 protected virtual async Task Update(CreateOrEditTeamDto input)
         {
            var team = await _teamRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, team);
         }

		 [AbpAuthorize(AppPermissions.Pages_Teams_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _teamRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTeamsToExcel(GetAllTeamsForExcelInput input)
         {
			
			var filteredTeams = _teamRepository.GetAll()
						.Include( e => e.SysStatusFk)
						.Include( e => e.ReferenceTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeNameFilter), e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == input.ReferenceTypeNameFilter);

			var query = (from o in filteredTeams
                         join o1 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_referenceTypeRepository.GetAll() on o.ReferenceTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTeamForViewDto() { 
							Team = new TeamDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	SysStatusName = s1 == null ? "" : s1.Name.ToString(),
                         	ReferenceTypeName = s2 == null ? "" : s2.Name.ToString()
						 });


            var teamListDtos = await query.ToListAsync();

            return _teamsExcelExporter.ExportToFile(teamListDtos);
         }

		[AbpAuthorize(AppPermissions.Pages_Teams)]
         public async Task<PagedResultDto<TeamSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TeamSysStatusLookupTableDto>();
			foreach(var sysStatus in sysStatusList){
				lookupTableDtoList.Add(new TeamSysStatusLookupTableDto
				{
					Id = sysStatus.Id,
					DisplayName = sysStatus.Name?.ToString()
				});
			}

            return new PagedResultDto<TeamSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Teams)]
         public async Task<PagedResultDto<TeamReferenceTypeLookupTableDto>> GetAllReferenceTypeForLookupTable(GetAllForLookupTableInput input)
         {
            var query = _lookup_referenceTypeRepository.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => (e.Name != null ? e.Name.ToString() : "").Contains(input.Filter))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeGroupFilter), e => e.ReferenceTypeGroup == input.ReferenceTypeGroupFilter);


            var totalCount = await query.CountAsync();

            var referenceTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TeamReferenceTypeLookupTableDto>();
			foreach(var referenceType in referenceTypeList){
				lookupTableDtoList.Add(new TeamReferenceTypeLookupTableDto
				{
					Id = referenceType.Id,
					DisplayName = referenceType.Name?.ToString()
				});
			}

            return new PagedResultDto<TeamReferenceTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}