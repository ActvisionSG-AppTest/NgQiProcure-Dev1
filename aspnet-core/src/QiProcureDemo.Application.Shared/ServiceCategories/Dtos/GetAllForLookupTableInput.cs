using Abp.Application.Services.Dto;

namespace QiProcureDemo.ServiceCategories.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}