using Abp.Application.Services.Dto;

namespace QiProcureDemo.ApprovalRequests.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}