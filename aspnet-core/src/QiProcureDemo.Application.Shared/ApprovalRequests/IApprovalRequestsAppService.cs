using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ApprovalRequests.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.ApprovalRequests
{
    public interface IApprovalRequestsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetApprovalRequestForViewDto>> GetAll(GetAllApprovalRequestsInput input);

        Task<GetApprovalRequestForViewDto> GetApprovalRequestForView(int id);

		Task<GetApprovalRequestForEditOutput> GetApprovalRequestForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditApprovalRequestDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetApprovalRequestsToExcel(GetAllApprovalRequestsForExcelInput input);

		
		Task<PagedResultDto<ApprovalRequestSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalRequestSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ApprovalRequestUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}