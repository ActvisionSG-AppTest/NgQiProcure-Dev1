using QiProcureDemo.SysRefs;
using QiProcureDemo.SysStatuses;
using QiProcureDemo.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ApprovalRequests.Exporting;
using QiProcureDemo.ApprovalRequests.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.ApprovalRequests.Dtos.Custom;
using QiProcureDemo.Authorization.Users.Profile;

namespace QiProcureDemo.ApprovalRequests
{
	[AbpAuthorize(AppPermissions.Pages_ApprovalRequests)]
    public class ApprovalRequestsAppService : QiProcureDemoAppServiceBase, IApprovalRequestsAppService
    {
		 private readonly IRepository<ApprovalRequest> _approvalRequestRepository;
		 private readonly IApprovalRequestsExcelExporter _approvalRequestsExcelExporter;
		 private readonly IRepository<SysRef,int> _lookup_sysRefRepository;
		 private readonly IRepository<SysStatus,int> _lookup_sysStatusRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
         private readonly ProfileAppService _profileAppService;


        public ApprovalRequestsAppService(IRepository<ApprovalRequest> approvalRequestRepository, IApprovalRequestsExcelExporter approvalRequestsExcelExporter ,
            IRepository<SysRef, int> lookup_sysRefRepository, IRepository<SysStatus, int> lookup_sysStatusRepository, IRepository<User, long> lookup_userRepository,
             ProfileAppService profileAppService) 
		  {
			_approvalRequestRepository = approvalRequestRepository;
			_approvalRequestsExcelExporter = approvalRequestsExcelExporter;
			_lookup_sysRefRepository = lookup_sysRefRepository;
		    _lookup_sysStatusRepository = lookup_sysStatusRepository;
		    _lookup_userRepository = lookup_userRepository;
            _profileAppService = profileAppService;
        }

		 public async Task<PagedResultDto<GetApprovalRequestForViewDto>> GetAll(GetAllApprovalRequestsInput input)
         {
			
			var filteredApprovalRequests = _approvalRequestRepository.GetAll()
						.Include( e => e.SysRefFk)
						.Include( e => e.SysStatusFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Remark.Contains(input.Filter))
						.WhereIf(input.MinReferenceIdFilter != null, e => e.ReferenceId >= input.MinReferenceIdFilter)
						.WhereIf(input.MaxReferenceIdFilter != null, e => e.ReferenceId <= input.MaxReferenceIdFilter)
						.WhereIf(input.MinOrderNoFilter != null, e => e.OrderNo >= input.MinOrderNoFilter)
						.WhereIf(input.MaxOrderNoFilter != null, e => e.OrderNo <= input.MaxOrderNoFilter)
						.WhereIf(input.MinRankNoFilter != null, e => e.RankNo >= input.MinRankNoFilter)
						.WhereIf(input.MaxRankNoFilter != null, e => e.RankNo <= input.MaxRankNoFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredApprovalRequests = filteredApprovalRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var approvalRequests = from o in pagedAndFilteredApprovalRequests
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetApprovalRequestForViewDto() {
							ApprovalRequest = new ApprovalRequestDto
							{
                                ReferenceId = o.ReferenceId,
                                OrderNo = o.OrderNo,
                                RankNo = o.RankNo,
                                Amount = o.Amount,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                         	SysStatusName = s2 == null ? "" : s2.Name.ToString(),
                         	UserName = s3 == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredApprovalRequests.CountAsync();

            return new PagedResultDto<GetApprovalRequestForViewDto>(
                totalCount,
                await approvalRequests.ToListAsync()
            );
         }
		 
		 public async Task<GetApprovalRequestForViewDto> GetApprovalRequestForView(int id)
         {
            var approvalRequest = await _approvalRequestRepository.GetAsync(id);

            var output = new GetApprovalRequestForViewDto { ApprovalRequest = ObjectMapper.Map<ApprovalRequestDto>(approvalRequest) };

		    if (output.ApprovalRequest.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.ApprovalRequest.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.ApprovalRequest.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.ApprovalRequest.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }

		    if (output.ApprovalRequest.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ApprovalRequest.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

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


        [AbpAuthorize(AppPermissions.Pages_ApprovalRequests_Edit)]
		 public async Task<GetApprovalRequestForEditOutput> GetApprovalRequestForEdit(EntityDto input)
         {
            var approvalRequest = await _approvalRequestRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetApprovalRequestForEditOutput {ApprovalRequest = ObjectMapper.Map<CreateOrEditApprovalRequestDto>(approvalRequest)};

		    if (output.ApprovalRequest.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.ApprovalRequest.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.ApprovalRequest.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.ApprovalRequest.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }

		    if (output.ApprovalRequest.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ApprovalRequest.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditApprovalRequestDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ApprovalRequests_Create)]
		 protected virtual async Task Create(CreateOrEditApprovalRequestDto input)
         {
            var approvalRequest = ObjectMapper.Map<ApprovalRequest>(input);

			
			if (AbpSession.TenantId != null)
			{
				approvalRequest.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _approvalRequestRepository.InsertAsync(approvalRequest);
         }

		 [AbpAuthorize(AppPermissions.Pages_ApprovalRequests_Edit)]
		 protected virtual async Task Update(CreateOrEditApprovalRequestDto input)
         {
            var approvalRequest = await _approvalRequestRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, approvalRequest);
         }

		 [AbpAuthorize(AppPermissions.Pages_ApprovalRequests_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _approvalRequestRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetApprovalRequestsToExcel(GetAllApprovalRequestsForExcelInput input)
         {
			
			var filteredApprovalRequests = _approvalRequestRepository.GetAll()
						.Include( e => e.SysRefFk)
						.Include( e => e.SysStatusFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Remark.Contains(input.Filter))
						.WhereIf(input.MinReferenceIdFilter != null, e => e.ReferenceId >= input.MinReferenceIdFilter)
						.WhereIf(input.MaxReferenceIdFilter != null, e => e.ReferenceId <= input.MaxReferenceIdFilter)
						.WhereIf(input.MinOrderNoFilter != null, e => e.OrderNo >= input.MinOrderNoFilter)
						.WhereIf(input.MaxOrderNoFilter != null, e => e.OrderNo <= input.MaxOrderNoFilter)
						.WhereIf(input.MinRankNoFilter != null, e => e.RankNo >= input.MinRankNoFilter)
						.WhereIf(input.MaxRankNoFilter != null, e => e.RankNo <= input.MaxRankNoFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredApprovalRequests
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetApprovalRequestForViewDto() { 
							ApprovalRequest = new ApprovalRequestDto
							{
                                ReferenceId = o.ReferenceId,
                                OrderNo = o.OrderNo,
                                RankNo = o.RankNo,
                                Amount = o.Amount,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                         	SysStatusName = s2 == null ? "" : s2.Name.ToString(),
                         	UserName = s3 == null ? "" : s3.Name.ToString()
						 });


            var approvalRequestListDtos = await query.ToListAsync();

            return _approvalRequestsExcelExporter.ExportToFile(approvalRequestListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ApprovalRequests)]
         public async Task<PagedResultDto<ApprovalRequestSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysRefRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.TenantId != null ? e.TenantId.ToString() : "").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ApprovalRequestSysRefLookupTableDto>();
			foreach(var sysRef in sysRefList){
				lookupTableDtoList.Add(new ApprovalRequestSysRefLookupTableDto
				{
					Id = sysRef.Id,
					DisplayName = sysRef.TenantId?.ToString()
				});
			}

            return new PagedResultDto<ApprovalRequestSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ApprovalRequests)]
         public async Task<PagedResultDto<ApprovalRequestSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ApprovalRequestSysStatusLookupTableDto>();
			foreach(var sysStatus in sysStatusList){
				lookupTableDtoList.Add(new ApprovalRequestSysStatusLookupTableDto
				{
					Id = sysStatus.Id,
					DisplayName = sysStatus.Name?.ToString()
				});
			}

            return new PagedResultDto<ApprovalRequestSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ApprovalRequests)]
         public async Task<PagedResultDto<ApprovalRequestUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ApprovalRequestUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ApprovalRequestUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ApprovalRequestUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}