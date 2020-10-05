using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProductImages.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}