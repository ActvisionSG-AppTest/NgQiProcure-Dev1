using QiProcureDemo.SysRefs;
using QiProcureDemo.Teams;
using QiProcureDemo.Projects;
using QiProcureDemo.Accounts;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.SysStatuses;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Approvals.Exporting;
using QiProcureDemo.Approvals.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.ApprovalRequests;
using QiProcureDemo.Authorization.Users.Dto;
using QiProcureDemo.ApprovalRequests.Dtos.Custom;
using QiProcureDemo.Common.Dto.Custom;
//using QiProcureDemo.ApprovalRequests.Dtos;

namespace QiProcureDemo.Approvals
{
    [AbpAuthorize(AppPermissions.Pages_Approvals)]
    public class ApprovalsAppService : QiProcureDemoAppServiceBase, IApprovalsAppService
    {
        private readonly IRepository<Approval> _approvalRepository;
        private readonly IApprovalsExcelExporter _approvalsExcelExporter;
        private readonly IRepository<SysRef, int> _lookup_sysRefRepository;
        private readonly IRepository<Team, int> _lookup_teamRepository;
        private readonly IRepository<Project, int> _lookup_projectRepository;
        private readonly IRepository<Account, int> _lookup_accountRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<SysStatus, int> _lookup_sysStatusRepository;
        private readonly IRepository<ApprovalRequest> _approvalRequestRepository;
        private readonly IRepository<Team, int> _teamRepository;
        private readonly IApprovalEmailer _approvalEmailer;
        private readonly ISysStatusesAppService _sysStatuses;

        public ApprovalsAppService(IRepository<Approval> approvalRepository, IApprovalsExcelExporter approvalsExcelExporter, IRepository<SysRef, int> lookup_sysRefRepository,
            IRepository<Team, int> lookup_teamRepository, IRepository<Project, int> lookup_projectRepository, IRepository<Account, int> lookup_accountRepository, 
            IRepository<User, long> lookup_userRepository, IRepository<SysStatus, int> lookup_sysStatusRepository, IRepository<ApprovalRequest> approvalRequestRepository, 
            IRepository<Team, int> teamRepository,IApprovalEmailer approvalEmailer, ISysStatusesAppService sysStatuses)
        {
            _approvalRepository = approvalRepository;
            _approvalsExcelExporter = approvalsExcelExporter;
            _lookup_sysRefRepository = lookup_sysRefRepository;
            _lookup_teamRepository = lookup_teamRepository;
            _lookup_projectRepository = lookup_projectRepository;
            _lookup_accountRepository = lookup_accountRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_sysStatusRepository = lookup_sysStatusRepository;
            _approvalRequestRepository = approvalRequestRepository;
            _teamRepository = teamRepository;
            _approvalEmailer = approvalEmailer;
            _sysStatuses = sysStatuses;
        }

        public async Task<PagedResultDto<GetApprovalForViewDto>> GetAll(GetAllApprovalsInput input)
        {

            var filteredApprovals = _approvalRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .Include(e => e.TeamFk)
                        .Include(e => e.ProjectFk)
                        .Include(e => e.AccountFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.SysStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinRankNoFilter != null, e => e.RankNo >= input.MinRankNoFilter)
                        .WhereIf(input.MaxRankNoFilter != null, e => e.RankNo <= input.MaxRankNoFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
/*						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefTenantIdFilter), e => e.SysRefFk != null && e.SysRefFk.TenantId == input.SysRefTenantIdFilter)
*/                        .WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameFilter), e => e.ProjectFk != null && e.ProjectFk.Name == input.ProjectNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountFk != null && e.AccountFk.Name == input.AccountNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

            var pagedAndFilteredApprovals = filteredApprovals
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var approvals = from o in pagedAndFilteredApprovals
                            join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_teamRepository.GetAll() on o.TeamId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            join o3 in _lookup_projectRepository.GetAll() on o.ProjectId equals o3.Id into j3
                            from s3 in j3.DefaultIfEmpty()

                            join o4 in _lookup_accountRepository.GetAll() on o.AccountId equals o4.Id into j4
                            from s4 in j4.DefaultIfEmpty()

                            join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                            from s5 in j5.DefaultIfEmpty()

                            join o6 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o6.Id into j6
                            from s6 in j6.DefaultIfEmpty()

                            select new GetApprovalForViewDto()
                            {
                                Approval = new ApprovalDto
                                {
                                    RankNo = o.RankNo,
                                    Amount = o.Amount,
                                    Id = o.Id
                                },
                                SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                TeamName = s2 == null ? "" : s2.Name.ToString(),
                                ProjectName = s3 == null ? "" : s3.Name.ToString(),
                                AccountName = s4 == null ? "" : s4.Name.ToString(),
                                UserName = s5 == null ? "" : s5.Name.ToString(),
                                SysStatusName = s6 == null ? "" : s6.Name.ToString(),
                                RefCode = s1 == null ? "" : s1.RefCode.ToString(),
                            };

            var totalCount = await filteredApprovals.CountAsync();

            return new PagedResultDto<GetApprovalForViewDto>(
                totalCount,
                await approvals.ToListAsync()
            );
        }

        public async Task<GetApprovalForViewDto> GetApprovalForView(int id)
        {
            var approval = await _approvalRepository.GetAsync(id);

            var output = new GetApprovalForViewDto { Approval = ObjectMapper.Map<ApprovalDto>(approval) };

            if (output.Approval.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Approval.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

            if (output.Approval.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.Approval.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

            if (output.Approval.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.Approval.ProjectId);
                output.ProjectName = _lookupProject.Name.ToString();
            }

            if (output.Approval.AccountId != null)
            {
                var _lookupAccount = await _lookup_accountRepository.FirstOrDefaultAsync((int)output.Approval.AccountId);
                output.AccountName = _lookupAccount.Name.ToString();
            }

            if (output.Approval.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Approval.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

            if (output.Approval.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Approval.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals_Edit)]
        public async Task<GetApprovalForEditOutput> GetApprovalForEdit(EntityDto input)
        {
            var approval = await _approvalRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApprovalForEditOutput { Approval = ObjectMapper.Map<CreateOrEditApprovalDto>(approval) };

            if (output.Approval.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Approval.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

            if (output.Approval.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.Approval.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

            if (output.Approval.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.Approval.ProjectId);
                output.ProjectName = _lookupProject.Name.ToString();
            }

            if (output.Approval.AccountId != null)
            {
                var _lookupAccount = await _lookup_accountRepository.FirstOrDefaultAsync((int)output.Approval.AccountId);
                output.AccountName = _lookupAccount.Name.ToString();
            }

            if (output.Approval.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Approval.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

            if (output.Approval.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Approval.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApprovalDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals_Create)]
        protected virtual async Task Create(CreateOrEditApprovalDto input)
        {
            var approval = ObjectMapper.Map<Approval>(input);


            if (AbpSession.TenantId != null)
            {
                approval.TenantId = (int?)AbpSession.TenantId;
            }


            await _approvalRepository.InsertAsync(approval);
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals_Edit)]
        protected virtual async Task Update(CreateOrEditApprovalDto input)
        {
            var approval = await _approvalRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, approval);
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _approvalRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetApprovalsToExcel(GetAllApprovalsForExcelInput input)
        {

            var filteredApprovals = _approvalRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .Include(e => e.TeamFk)
                        .Include(e => e.ProjectFk)
                        .Include(e => e.AccountFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.SysStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinRankNoFilter != null, e => e.RankNo >= input.MinRankNoFilter)
                        .WhereIf(input.MaxRankNoFilter != null, e => e.RankNo <= input.MaxRankNoFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
/*						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefTenantIdFilter), e => e.SysRefFk != null && e.SysRefFk.TenantId == input.SysRefTenantIdFilter)
*/                        .WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameFilter), e => e.ProjectFk != null && e.ProjectFk.Name == input.ProjectNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountFk != null && e.AccountFk.Name == input.AccountNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

            var query = (from o in filteredApprovals
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_teamRepository.GetAll() on o.TeamId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_projectRepository.GetAll() on o.ProjectId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_accountRepository.GetAll() on o.AccountId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         select new GetApprovalForViewDto()
                         {
                             Approval = new ApprovalDto
                             {
                                 RankNo = o.RankNo,
                                 Amount = o.Amount,
                                 Id = o.Id
                             },
                             SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                             TeamName = s2 == null ? "" : s2.Name.ToString(),
                             ProjectName = s3 == null ? "" : s3.Name.ToString(),
                             AccountName = s4 == null ? "" : s4.Name.ToString(),
                             UserName = s5 == null ? "" : s5.Name.ToString(),
                             SysStatusName = s6 == null ? "" : s6.Name.ToString()
                         });


            var approvalListDtos = await query.ToListAsync();

            return _approvalsExcelExporter.ExportToFile(approvalListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_Approvals)]
        public async Task<PagedResultDto<ApprovalSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_sysRefRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.TenantId != null ? e.TenantId.ToString() : "").Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ApprovalSysRefLookupTableDto>();
            foreach (var sysRef in sysRefList)
            {
                lookupTableDtoList.Add(new ApprovalSysRefLookupTableDto
                {
                    Id = sysRef.Id,
                    DisplayName = sysRef.TenantId?.ToString()
                });
            }

            return new PagedResultDto<ApprovalSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals)]
        public async Task<PagedResultDto<ApprovalTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_teamRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null ? e.Name.ToString() : "").Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var teamList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ApprovalTeamLookupTableDto>();
            foreach (var team in teamList)
            {
                lookupTableDtoList.Add(new ApprovalTeamLookupTableDto
                {
                    Id = team.Id,
                    DisplayName = team.Name?.ToString()
                });
            }

            return new PagedResultDto<ApprovalTeamLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals)]
        public async Task<PagedResultDto<ApprovalProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_projectRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null ? e.Name.ToString() : "").Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var projectList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ApprovalProjectLookupTableDto>();
            foreach (var project in projectList)
            {
                lookupTableDtoList.Add(new ApprovalProjectLookupTableDto
                {
                    Id = project.Id,
                    DisplayName = project.Name?.ToString()
                });
            }

            return new PagedResultDto<ApprovalProjectLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals)]
        public async Task<PagedResultDto<ApprovalAccountLookupTableDto>> GetAllAccountForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_accountRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null ? e.Name.ToString() : "").Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var accountList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ApprovalAccountLookupTableDto>();
            foreach (var account in accountList)
            {
                lookupTableDtoList.Add(new ApprovalAccountLookupTableDto
                {
                    Id = account.Id,
                    DisplayName = account.Name?.ToString()
                });
            }

            return new PagedResultDto<ApprovalAccountLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals)]
        public async Task<PagedResultDto<ApprovalUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null ? e.Name.ToString() : "").Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ApprovalUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new ApprovalUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<ApprovalUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Approvals)]
        public async Task<PagedResultDto<ApprovalSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null ? e.Name.ToString() : "").Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ApprovalSysStatusLookupTableDto>();
            foreach (var sysStatus in sysStatusList)
            {
                lookupTableDtoList.Add(new ApprovalSysStatusLookupTableDto
                {
                    Id = sysStatus.Id,
                    DisplayName = sysStatus.Name?.ToString()
                });
            }

            return new PagedResultDto<ApprovalSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<RequestOutput> SubmitNewTeamForApproval(SubmitNewTeamForApprovalInput input)
        {
            //check if there is an approver for the team      
            var queryteam = _lookup_teamRepository.GetAll()
                .Include(e => e.SysStatusFk)
                .Where(e => e.Id == input.TeamId);

            var teams = await queryteam
               .ToListAsync();

            if (teams == null)
            {
                throw new Exception(L("RecordNotFound"));
            }
            else
            {
                //check the Team Status
                foreach (var team in teams)
                {
                    if (team.SysStatusFk.Name.ToLower() != "new")
                    {
                        throw new Exception(L("InvalidSubmitApprovalStatus"));
                    }
                    else
                    {
                        var nextStatusDto = _sysStatuses.GetNextSysStatus(team.SysStatusFk.Code, "approval", "");

                        var inputTeam = ObjectMapper.Map<Team>(team);
                        inputTeam.SysStatusId = nextStatusDto.SysStatus.Id;

                        var teamUpdate = await _teamRepository.FirstOrDefaultAsync((int)inputTeam.Id);
                        ObjectMapper.Map(inputTeam, teamUpdate);

                    }
                }

            }

            var queryApproval = _approvalRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .Include(e => e.UserFk)
                        .Where(e => e.SysRefFk.RefCode == "Team");

            var approvalList = await queryApproval
                .OrderBy("RankNo, Amount asc")
                .ToListAsync();

            var count = 0;

            var requestOutput = new RequestOutput();

            /* get approval status reference Id */
            var queryStatusRef= _lookup_sysRefRepository.GetAll()
               .Include(e => e.ReferenceTypeFk)
               .Where(e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == "Status")
               .Where(e => e.RefCode == "approval");
            
            var statusRefList = await queryStatusRef.ToListAsync();

            int statusRefId = 0;
            foreach (var statusRef in statusRefList)
            {
                statusRefId = statusRef.Id;
            }
            /* -----------------------**/
            /* get approval status Id */
            var queryStatus = _lookup_sysStatusRepository.GetAll()
               .Where(e => e.SysRefId == statusRefId);

            var statusList = await queryStatus.ToListAsync();

            int newStatusId = 0;int approvalStatusId = 0; int statusCount = 0;
            foreach (var status in statusList)
            {
                if (statusCount == 1) { newStatusId = status.Id; }; /* Require Approval*/
                if (statusCount == 2) { approvalStatusId = status.Id; }; /* Pending Approval*/
                statusCount += 1;
            }
            /* -----------------------**/

            if (newStatusId != 0 && approvalStatusId != 0)
            {
                foreach (var approval in approvalList)
                {

                    var queryApprovalRequesUser = _approvalRequestRepository.GetAll()
                                .Where(e => e.ReferenceId == input.TeamId && e.UserId == approval.UserId);

                    var approvalRequestUserList = await queryApprovalRequesUser
                        .ToListAsync();

                    if (approvalRequestUserList.Count == 0)
                    {
                        // insert into approvalRequest
                        var approvalUserId = (approval.UserId == null) ? 0 : approval.UserId.Value;
                        count += 1;
                        
                        int statusId = newStatusId;
                        if (count == 1){ statusId = approvalStatusId; };
                        
                        var _approvalRequest = new ApprovalRequest
                        {
                            RankNo = approval.RankNo,
                            Amount = approval.Amount,
                            SysRefId = approval.SysRefId,
                            ReferenceId = input.TeamId,
                            UserId = approvalUserId,
                            SysStatusId = statusId,
                            OrderNo = count,

                        };


                        var user = new User
                        {
                            Name = approval.UserFk.Name,
                            Surname = approval.UserFk.Surname,
                            EmailAddress = approval.UserFk.EmailAddress,
                            IsActive = approval.UserFk.IsActive,
                            UserName = approval.UserFk.UserName,
                            IsEmailConfirmed = approval.UserFk.IsEmailConfirmed
                        };

                        //create approval requestM
                        var approvalRequest = ObjectMapper.Map<ApprovalRequest>(_approvalRequest);
                        if (AbpSession.TenantId != null){approvalRequest.TenantId = (int?)AbpSession.TenantId;}
                        await _approvalRequestRepository.InsertAsync(approvalRequest);

                        //send request for approval from first approver
                        if (count == 1)
                        {
                            await _approvalEmailer.SendEmailApprovalAsync(approvalRequest, user, "");
                            requestOutput.Status = true;
                            requestOutput.StatusMessage = "SUCCESS";

                        }
                    }
                }
            }
                      
            return requestOutput;
        }


    }
}