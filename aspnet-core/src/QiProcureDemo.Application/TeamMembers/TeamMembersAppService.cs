using QiProcureDemo.Teams;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.SysRefs;
using QiProcureDemo.SysStatuses;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.TeamMembers.Exporting;
using QiProcureDemo.TeamMembers.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.Authorization.Users.Profile;

namespace QiProcureDemo.TeamMembers
{
	[AbpAuthorize(AppPermissions.Pages_TeamMembers)]
    public class TeamMembersAppService : QiProcureDemoAppServiceBase, ITeamMembersAppService
    {
		 private readonly IRepository<TeamMember> _teamMemberRepository;
		 private readonly ITeamMembersExcelExporter _teamMembersExcelExporter;
		 private readonly IRepository<Team,int> _lookup_teamRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<SysRef,int> _lookup_sysRefRepository;
		 private readonly IRepository<SysStatus,int> _lookup_sysStatusRepository;
         private readonly ProfileAppService _profileAppService;

        public TeamMembersAppService(IRepository<TeamMember> teamMemberRepository, ITeamMembersExcelExporter teamMembersExcelExporter ,
              IRepository<Team, int> lookup_teamRepository, IRepository<User, long> lookup_userRepository, 
              IRepository<SysRef, int> lookup_sysRefRepository, IRepository<SysStatus, int> lookup_sysStatusRepository,
              ProfileAppService profileAppService
              ) 
		  {
			_teamMemberRepository = teamMemberRepository;
			_teamMembersExcelExporter = teamMembersExcelExporter;
			_lookup_teamRepository = lookup_teamRepository;
		    _lookup_userRepository = lookup_userRepository;
		    _lookup_sysRefRepository = lookup_sysRefRepository;
		    _lookup_sysStatusRepository = lookup_sysStatusRepository;
            _profileAppService = profileAppService;
        }

		 public async Task<PagedResultDto<GetTeamMemberForViewDto>> GetAll(GetAllTeamMembersInput input)
         {
			
			var filteredTeamMembers = _teamMemberRepository.GetAll()
						.Include( e => e.TeamFk)
						.Include( e => e.UserFk)
						.Include( e => e.SysRefFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(input.MinReportingTeamMemberIdFilter != null, e => e.ReportingTeamMemberId >= input.MinReportingTeamMemberIdFilter)
						.WhereIf(input.MaxReportingTeamMemberIdFilter != null, e => e.ReportingTeamMemberId <= input.MaxReportingTeamMemberIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
//						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefTenantIdFilter), e => e.SysRefFk != null && e.SysRefFk.TenantId == input.SysRefTenantIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var pagedAndFilteredTeamMembers = filteredTeamMembers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var teamMembers = from o in pagedAndFilteredTeamMembers
                         join o1 in _lookup_teamRepository.GetAll() on o.TeamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o3.Id into j3                         
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                                                                                         
                         select new GetTeamMemberForViewDto() {
							TeamMember = new TeamMemberDto
							{
                                Remark = o.Remark,
                                ReportingTeamMemberId = o.ReportingTeamMemberId,
                                Id = o.Id
							},
                         	TeamName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString(),
                         	SysRefTenantId = s3 == null ? "" : s3.TenantId.ToString(),
                         	SysStatusName = s4 == null ? "" : s4.Name.ToString()
						};
            var totalCount = await filteredTeamMembers.CountAsync();

            return new PagedResultDto<GetTeamMemberForViewDto>(
                totalCount,
                await teamMembers.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetTeamMemberForViewDto>> GetTeamMembers(GetAllTeamMembersInput input)
        {

            var filteredTeamMembers = _teamMemberRepository.GetAll()
                        .Include(e => e.TeamFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.SysRefFk)
                        .Include(e => e.SysStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remark.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter), e => e.Remark == input.RemarkFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(input.ReportingTeamMemberIdFilter != null, e => e.ReportingTeamMemberId == input.ReportingTeamMemberIdFilter)
                        .WhereIf(input.TeamIdFilter != null, e => e.TeamFk.Id == input.TeamIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

            var pagedAndFilteredTeamMembers = filteredTeamMembers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var teamMembers = from o in pagedAndFilteredTeamMembers
                              join o1 in _lookup_teamRepository.GetAll() on o.TeamId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              join o3 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o3.Id into j3
                              from s3 in j3.DefaultIfEmpty()

                              join o4 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o4.Id into j4
                              from s4 in j4.DefaultIfEmpty()
                              
                              
                              orderby o.SysRefFk.OrderNumber

                              select new GetTeamMemberForViewDto()
                              {
                                  TeamMember = new TeamMemberDto
                                  {
                                      Remark = o.Remark,
                                      ReportingTeamMemberId = o.ReportingTeamMemberId,
                                      Id = o.Id
                                  },
                                  TeamName = s1 == null ? "" : s1.Name.ToString(),
                                  UserName = s2 == null ? "" : s2.Name.ToString(),
                                  SysRefTenantId = s3 == null ? "" : s3.TenantId.ToString(),
                                  SysStatusName = s4 == null ? "" : s4.Name.ToString(),
                                  FullName = s2 == null ? "" : s2.FullName.ToString(),
                                  EmailAddress = s2 == null ? "" : s2.EmailAddress.ToString(),
                                  SelectedRoleName = s3 == null ? "" : s3.RefCode.ToString(),
                                  ProfilePictureId = s2 == null ? Guid.Empty : s2.ProfilePictureId ,
                                  ProfilePicture = ""
                                  
                              };

            var teamMemberListDtos = await teamMembers.ToListAsync();
            foreach (var teamMember in teamMemberListDtos)
            {
                Guid profilePictureId = teamMember.ProfilePictureId.Value;
                teamMember.ProfilePicture = await _profileAppService.GetProfilePictureByIdString(profilePictureId);
            }

            var totalCount = await filteredTeamMembers.CountAsync();

            return new PagedResultDto<GetTeamMemberForViewDto>(
                totalCount,
                teamMemberListDtos
            );
        }

        public async Task<GetTeamMemberForViewDto> GetTeamMemberForView(int id)
         {
            var teamMember = await _teamMemberRepository.GetAsync(id);

            var output = new GetTeamMemberForViewDto { TeamMember = ObjectMapper.Map<TeamMemberDto>(teamMember) };

		    if (output.TeamMember.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.TeamMember.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

		    if (output.TeamMember.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TeamMember.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.TeamMember.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.TeamMember.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.TeamMember.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.TeamMember.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TeamMembers_Edit)]
		 public async Task<GetTeamMemberForEditOutput> GetTeamMemberForEdit(EntityDto input)
         {
            var teamMember = await _teamMemberRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTeamMemberForEditOutput {TeamMember = ObjectMapper.Map<CreateOrEditTeamMemberDto>(teamMember)};

		    if (output.TeamMember.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.TeamMember.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

		    if (output.TeamMember.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TeamMember.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.TeamMember.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.TeamMember.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.TeamMember.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.TeamMember.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }


		 public async Task CreateOrEdit(CreateOrEditTeamMemberDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TeamMembers_Create)]
		 protected virtual async Task Create(CreateOrEditTeamMemberDto input)
         {

            var query = _teamMemberRepository.GetAll()
                        .Where(e => e.TeamId == input.TeamId && e.UserId == input.UserId);

            var userCount = await query.CountAsync();

            if (userCount == 0)
            {
                var teamMember = ObjectMapper.Map<TeamMember>(input);

                if (AbpSession.TenantId != null)
                {
                    teamMember.TenantId = (int?)AbpSession.TenantId;
                }


                await _teamMemberRepository.InsertAsync(teamMember);
            }
        }

		 [AbpAuthorize(AppPermissions.Pages_TeamMembers_Edit)]
		 protected virtual async Task Update(CreateOrEditTeamMemberDto input)
         {
            var teamMember = await _teamMemberRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, teamMember);
         }

		 [AbpAuthorize(AppPermissions.Pages_TeamMembers_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _teamMemberRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTeamMembersToExcel(GetAllTeamMembersForExcelInput input)
         {
			
			var filteredTeamMembers = _teamMemberRepository.GetAll()
						.Include( e => e.TeamFk)
						.Include( e => e.UserFk)
						.Include( e => e.SysRefFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(input.MinReportingTeamMemberIdFilter != null, e => e.ReportingTeamMemberId >= input.MinReportingTeamMemberIdFilter)
						.WhereIf(input.MaxReportingTeamMemberIdFilter != null, e => e.ReportingTeamMemberId <= input.MaxReportingTeamMemberIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefTenantIdFilter), e => e.SysRefFk != null && e.SysRefFk.TenantId == input.SysRefTenantIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var query = (from o in filteredTeamMembers
                         join o1 in _lookup_teamRepository.GetAll() on o.TeamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetTeamMemberForViewDto() { 
							TeamMember = new TeamMemberDto
							{
                                Remark = o.Remark,
                                ReportingTeamMemberId = o.ReportingTeamMemberId,
                                Id = o.Id
							},
                         	TeamName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString(),
                         	SysRefTenantId = s3 == null ? "" : s3.TenantId.ToString(),
                         	SysStatusName = s4 == null ? "" : s4.Name.ToString()
						 });


            var teamMemberListDtos = await query.ToListAsync();

            return _teamMembersExcelExporter.ExportToFile(teamMemberListDtos);
         }


		[AbpAuthorize(AppPermissions.Pages_TeamMembers)]
         public async Task<PagedResultDto<TeamMemberTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_teamRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var teamList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TeamMemberTeamLookupTableDto>();
			foreach(var team in teamList){
				lookupTableDtoList.Add(new TeamMemberTeamLookupTableDto
				{
					Id = team.Id,
					DisplayName = team.Name?.ToString()
				});
			}

            return new PagedResultDto<TeamMemberTeamLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_TeamMembers)]
         public async Task<PagedResultDto<TeamMemberUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TeamMemberUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new TeamMemberUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<TeamMemberUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_TeamMembers)]
         public async Task<PagedResultDto<TeamMemberSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysRefRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.TenantId != null ? e.TenantId.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TeamMemberSysRefLookupTableDto>();
			foreach(var sysRef in sysRefList){
				lookupTableDtoList.Add(new TeamMemberSysRefLookupTableDto
				{
					Id = sysRef.Id,
					DisplayName = sysRef.TenantId?.ToString()
				});
			}

            return new PagedResultDto<TeamMemberSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_TeamMembers)]
         public async Task<PagedResultDto<TeamMemberSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TeamMemberSysStatusLookupTableDto>();
			foreach(var sysStatus in sysStatusList){
				lookupTableDtoList.Add(new TeamMemberSysStatusLookupTableDto
				{
					Id = sysStatus.Id,
					DisplayName = sysStatus.Name?.ToString()
				});
			}

            return new PagedResultDto<TeamMemberSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}