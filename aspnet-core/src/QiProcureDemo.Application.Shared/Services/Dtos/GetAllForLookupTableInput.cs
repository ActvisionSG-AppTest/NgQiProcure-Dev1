using Abp.Application.Services.Dto;

namespace QiProcureDemo.Services.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}