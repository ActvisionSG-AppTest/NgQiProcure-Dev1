using Abp.Application.Services.Dto;

namespace QiProcureDemo.SysRefs.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}