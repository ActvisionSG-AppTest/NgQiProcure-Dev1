using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProductPrices.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}