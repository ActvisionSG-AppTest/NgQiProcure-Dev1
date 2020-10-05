using Abp.Application.Services.Dto;

namespace QiProcureDemo.ServiceImages.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}