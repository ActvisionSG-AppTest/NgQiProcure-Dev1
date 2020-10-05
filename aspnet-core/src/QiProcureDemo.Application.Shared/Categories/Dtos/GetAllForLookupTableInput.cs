using Abp.Application.Services.Dto;

namespace QiProcureDemo.Categories.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}