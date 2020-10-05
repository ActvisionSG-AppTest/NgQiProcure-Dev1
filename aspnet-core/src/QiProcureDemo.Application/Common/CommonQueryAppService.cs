using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Zero.Configuration;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.ApprovalRequests;
using QiProcureDemo.ApprovalRequests.Dtos;
using QiProcureDemo.ApprovalRequests.Dtos.Custom;
using QiProcureDemo.Authorization.Roles;
using QiProcureDemo.Authorization.Roles.Dto;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.Authorization.Users.Dto;
using QiProcureDemo.Authorization.Users.Profile;
using QiProcureDemo.Common.Dto.Custom;
using QiProcureDemo.Editions;
using QiProcureDemo.ParamSettings;
using QiProcureDemo.ParamSettings.Dtos;
using QiProcureDemo.ReferenceTypes;
using QiProcureDemo.SysRefs;
using QiProcureDemo.SysRefs.Dtos;
using QiProcureDemo.SysRefs.Dtos.Custom;
using QiProcureDemo.SysStatuses;
using QiProcureDemo.SysStatuses.Dtos;
using QiProcureDemo.TeamMembers;
using QiProcureDemo.TeamMembers.Dtos;
using QiProcureDemo.Teams;

namespace QiProcureDemo.Common
{
    [AbpAuthorize]
    public class CommonQueryAppService : QiProcureDemoAppServiceBase, ICommonQueryAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IUserAppService _userAppServce;
        private readonly IRepository<SysStatus> _SysStatusRepository;
        private readonly IRepository<SysStatus, int> _lookup_sysStatusRepository;
        private readonly IRepository<SysRef, int> _lookup_sysRefRepository;
        private readonly ProfileAppService _profileAppService;
        private readonly IRepository<TeamMember> _teamMemberRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<ParamSetting> _paramSettingRepository;
        private readonly IRepository<SysRef> _sysRefRepository;
        private readonly IRepository<ReferenceType, int> _lookup_referenceTypeRepository;
        private readonly IRepository<ApprovalRequest> _approvalRequestRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Team, int> _lookup_teamRepository;

        public CommonQueryAppService(RoleManager roleManager, IUserAppService userAppServce,
            IRepository<SysStatus> sysStatusRepository, 
            IRepository<SysRef, int> lookup_sysRefRepository,
            ProfileAppService profileAppService,
            IRepository<TeamMember> teamMemberRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<ParamSetting> paramSettingRepository,
            IRepository<SysRef> sysRefRepository,
            IRepository<ReferenceType, int> lookup_referenceTypeRepository,
            IRepository<ApprovalRequest> approvalRequestRepository,
            IRepository<SysStatus, int> lookup_sysStatusRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<Team, int> lookup_teamRepository
        )
        {           
            _roleManager = roleManager;
            _userAppServce = userAppServce;
            _lookup_userRepository = lookup_userRepository;
            _SysStatusRepository = sysStatusRepository;
            _lookup_sysStatusRepository = lookup_sysStatusRepository;
            _lookup_sysRefRepository = lookup_sysRefRepository;
            _profileAppService = profileAppService;
            _teamMemberRepository = teamMemberRepository;
            _userRoleRepository = userRoleRepository;
            _paramSettingRepository = paramSettingRepository;
            _sysRefRepository = sysRefRepository;
            _lookup_referenceTypeRepository = lookup_referenceTypeRepository;
            _approvalRequestRepository = approvalRequestRepository;
            _lookup_teamRepository = lookup_teamRepository;
        }

        /* Get List of Roles*/
        public ListResultDto<RoleListDto> GetRoles()
        {
            var query = _roleManager.Roles;          

            var roles = query.ToList();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        #region Common Query        
        /* Is User Admin*/
        public RequestOutput GetIsUserAdmin(GeneralInput input)
        {
            var query = _roleManager.Roles;
            var roles = query.ToList();

            var requestOutput = new RequestOutput();

            foreach (var role in roles)
            {
                if (role.Name.ToLower() == "admin")
                {
                    var userInputDto = new GetUsersInput();
                    userInputDto.Filter = input.UserName;
                    userInputDto.Role = role.Id;

                    var queryUser = UserManager.Users
                    .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                    .WhereIf(!input.UserName.IsNullOrWhiteSpace(),u =>u.UserName.Contains(input.UserName));

                    var userCount = queryUser.Count();

                    if (userCount > 0)
                    {
                        requestOutput.Status = true;
                    }
                    requestOutput.StatusMessage = "SUCCESS";
                    break;
                }
            }
                      
            return requestOutput;
        }

        #endregion Common

        #region System Status
        public async Task<PagedResultDto<GetSysStatusForViewDto>> GetAll(GetAllSysStatusesInput input)
        {

            var filteredSysStatuses = _SysStatusRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinCodeFilter != null, e => e.Code >= input.MinCodeFilter)
                        .WhereIf(input.MaxCodeFilter != null, e => e.Code <= input.MaxCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefCodeFilter), e => e.SysRefFk.RefCode == input.RefCodeFilter);

            var pagedAndFilteredSysStatuses = filteredSysStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var SysStatuses = from o in pagedAndFilteredSysStatuses
                              join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              select new GetSysStatusForViewDto()
                              {
                                  SysStatus = new SysStatusDto
                                  {
                                      Code = o.Code,
                                      Name = o.Name,
                                      Description = o.Description,
                                      Id = o.Id,
                                      RefCode = o.SysRefFk.RefCode
                                  },
                                  SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString()
                              };

            var totalCount = await filteredSysStatuses.CountAsync();

            return new PagedResultDto<GetSysStatusForViewDto>(
                totalCount,
                await SysStatuses.ToListAsync()
            );
        }

        #endregion System Status

        #region User
        public async Task<PagedResultDto<UserProfileListDto>> GetUserTeamProfiles(GetUsersTeamInput input)
        {
            var query = GetUsersTeamFilteredQuery(input, input.TeamId, input.isSelected);
            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .ToListAsync();


            var userProfileListDtos = ObjectMapper.Map<List<UserProfileListDto>>(users);

            foreach (var userProfileListDto in userProfileListDtos)
            {
                if (userProfileListDto.ProfilePictureId != null)
                {
                    userProfileListDto.ProfilePicture = await _profileAppService.GetProfilePictureByIdString(userProfileListDto.ProfilePictureId);


                    var teamMemberQuery = _teamMemberRepository.GetAll()
                                 .Where(e => e.TeamId == input.TeamId && e.UserId == userProfileListDto.Id);

                    var memberquery = from o in teamMemberQuery

                                      select new GetTeamMemberForViewDto()
                                      {
                                          TeamMember = new TeamMemberDto
                                          {
                                              Id = o.Id,
                                              SysRefId = o.SysRefId,
                                              ReportingTeamMemberId = o.ReportingTeamMemberId,
                                              RoleOrderNumber = o.SysRefFk.OrderNumber
                                          },
                                      };

                    var teamMemberListDtos = await memberquery
                        .ToListAsync();
                    foreach (var teamMember in teamMemberListDtos)
                    {
                        userProfileListDto.TeamMemberId = teamMember.TeamMember.Id;
                        userProfileListDto.SelectedRole = teamMember.TeamMember.SysRefId;
                        userProfileListDto.ReportingTo = teamMember.TeamMember.ReportingTeamMemberId;
                        userProfileListDto.RoleOrderNumber = teamMember.TeamMember.RoleOrderNumber;
                    }
                };
            }

            await FillRoleProfileNames(userProfileListDtos);

            return new PagedResultDto<UserProfileListDto>(
                userCount,
                userProfileListDtos
                );
        }
        private IQueryable<User> GetUsersTeamFilteredQuery(IGetUsersInput input, int teamId, bool isSelected)
        {
            var teamMemberQuery = _teamMemberRepository.GetAll()
                .Where(e => e.TeamId == teamId)
                .Select(member => member.UserId);

            var query = UserManager.Users
                .WhereIf(!isSelected, u => !teamMemberQuery.Contains(u.Id))
                .WhereIf(isSelected, u => teamMemberQuery.Contains(u.Id));

            return query;
        }

        private async Task FillRoleProfileNames(IReadOnlyCollection<UserProfileListDto> userPorifleListDtos)
        {
            /* This method is optimized to fill role names to given list. */
            var userIds = userPorifleListDtos.Select(u => u.Id);

            var userRoles = await _userRoleRepository.GetAll()
                .Where(userRole => userIds.Contains(userRole.UserId))
                .Select(userRole => userRole).ToListAsync();

            var distinctRoleIds = userRoles.Select(userRole => userRole.RoleId).Distinct();

            foreach (var user in userPorifleListDtos)
            {
                var rolesOfUser = userRoles.Where(userRole => userRole.UserId == user.Id).ToList();
                user.Roles = ObjectMapper.Map<List<UserListRoleDto>>(rolesOfUser);
            }

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    roleNames[roleId] = role.DisplayName;
                }
            }

            foreach (var userListDto in userPorifleListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                {
                    if (roleNames.ContainsKey(userListRoleDto.RoleId))
                    {
                        userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];
                    }
                }

                userListDto.Roles = userListDto.Roles.Where(r => r.RoleName != null).OrderBy(r => r.RoleName).ToList();
            }
        }

        #endregion User

        #region Param Setting

        public async Task<PagedResultDto<GetParamSettingForViewDto>> GetParamSetting_All(GetAllParamSettingsInput input)
        {

            var filteredParamSettings = _paramSettingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Value.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value == input.ValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredParamSettings = filteredParamSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var paramSettings = from o in pagedAndFilteredParamSettings
                                select new GetParamSettingForViewDto()
                                {
                                    ParamSetting = new ParamSettingDto
                                    {
                                        Name = o.Name,
                                        Value = o.Value,
                                        Description = o.Description,
                                        Id = o.Id
                                    }
                                };

            var totalCount = await filteredParamSettings.CountAsync();

            return new PagedResultDto<GetParamSettingForViewDto>(
                totalCount,
                await paramSettings.ToListAsync()
            );
        }
        #endregion Param setting

        #region SysRef
        public async Task<PagedResultDto<GetSysRefForViewDto>> GetSysRef_All(GetAllSysRefsInput input)
        {

            var filteredSysRefs = _sysRefRepository.GetAll()
                        .Include(e => e.ReferenceTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RefCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefCodeFilter), e => e.RefCode == input.RefCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.ReferenceTypeIdFilter != null, e => e.ReferenceTypeId == input.ReferenceTypeIdFilter)
                        .WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
                        .WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeNameFilter), e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == input.ReferenceTypeNameFilter);

            var pagedAndFilteredSysRefs = filteredSysRefs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var sysRefs = from o in pagedAndFilteredSysRefs
                          join o1 in _lookup_referenceTypeRepository.GetAll() on o.ReferenceTypeId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new GetSysRefForViewDto()
                          {
                              SysRef = new SysRefDto
                              {
                                  RefCode = o.RefCode,
                                  Description = o.Description,
                                  OrderNumber = o.OrderNumber,
                                  Id = o.Id
                              },
                              ReferenceTypeName = s1 == null ? "" : s1.Name.ToString()
                          };

            var totalCount = await filteredSysRefs.CountAsync();

            return new PagedResultDto<GetSysRefForViewDto>(
                totalCount,
                await sysRefs.ToListAsync()
            );
        }

        #endregion

        #region Approval Request
        public async Task<PagedResultDto<GetApprovalRequestForViewDto>> GetApprovalRequest(GetCustomApprovalRequestsInput input)
        {

            var filteredApprovalRequests = _approvalRequestRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .Include(e => e.SysStatusFk)
                        .Include(e => e.UserFk)
                        .WhereIf(input.ReferenceId != null, e => e.ReferenceId == input.ReferenceId)
                        .WhereIf(input.SysRefId != null, e => e.ReferenceId == input.SysRefId)
                        .WhereIf(input.SysStatusId != null, e => e.OrderNo == input.SysStatusId);

            var pagedAndFilteredApprovalRequests = filteredApprovalRequests
                .OrderBy("OrderNo, RankNo, Amount asc");

            var approvalRequests = from o in pagedAndFilteredApprovalRequests
                                   join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                                   from s3 in j3.DefaultIfEmpty()

                                   select new GetApprovalRequestForViewDto()
                                   {
                                       ApprovalRequest = new ApprovalRequestDto
                                       {
                                           ReferenceId = o.ReferenceId,
                                           OrderNo = o.OrderNo,
                                           UserId = o.UserId,
                                           RankNo = o.RankNo,
                                           Amount = o.Amount,
                                           Remark = o.Remark,
                                           Id = o.Id,
                                           SysStatusId = o.SysStatusId,
                                           SysStatusName = s2.Name.ToString()
                                       },

                                       SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                       SysStatusName = s2 == null ? "" : s2.Name.ToString(),
                                       UserName = s3 == null ? "" : s3.UserName.ToString(),
                                       FullName = s3 == null ? "" : s3.FullName.ToString(),
                                       EmailAddress = s3 == null ? "" : s3.EmailAddress.ToString(),
                                       ProfilePictureId = s3 == null ? Guid.Empty : s3.ProfilePictureId,
                                       ProfilePicture = ""
                                   };
            var totalCount = await filteredApprovalRequests.CountAsync();

            var approvalRequestsListDtos = await approvalRequests.ToListAsync();
            int orderNo = 0;
            foreach (var approvalRequest in approvalRequestsListDtos)
            {
                orderNo += 1;
                Guid profilePictureId = approvalRequest.ProfilePictureId.Value;
                approvalRequest.ProfilePicture = await _profileAppService.GetProfilePictureByIdString(profilePictureId);
                approvalRequest.ApprovalRequest.OrderNo = orderNo;
            }

            return new PagedResultDto<GetApprovalRequestForViewDto>(
               totalCount,
               approvalRequestsListDtos
            );
        }

        #endregion Approval Request

        #region Team Members
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
                                  ProfilePictureId = s2 == null ? Guid.Empty : s2.ProfilePictureId,
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

        #endregion
    }
}
