using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProductCategories.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}