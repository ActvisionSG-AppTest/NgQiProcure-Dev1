using Abp.Application.Services.Dto;

namespace QiProcureDemo.ServicePrices.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}