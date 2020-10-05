using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ApprovalRequests.Dtos.Custom;
using QiProcureDemo.Approvals.Dtos;
using QiProcureDemo.Common.Dto.Custom;
using QiProcureDemo.Dto;


namespace QiProcureDemo.Approvals
{
    public interface IApprovalsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetApprovalForViewDto>> GetAll(GetAllApprovalsInput input);

        Task<GetApprovalForViewDto> GetApprovalForView(int id);

		Task<GetApprovalForEditOutput> GetApprovalForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditApprovalDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetApprovalsToExcel(GetAllApprovalsForExcelInput input);

		
		Task<PagedResultDto<ApprovalSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalAccountLookupTableDto>> GetAllAccountForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);

		Task<RequestOutput> SubmitNewTeamForApproval(SubmitNewTeamForApprovalInput input);

	}
}