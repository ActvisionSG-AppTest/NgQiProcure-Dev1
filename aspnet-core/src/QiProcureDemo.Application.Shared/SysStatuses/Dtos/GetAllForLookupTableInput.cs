using Abp.Application.Services.Dto;

namespace QiProcureDemo.SysStatuses.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}