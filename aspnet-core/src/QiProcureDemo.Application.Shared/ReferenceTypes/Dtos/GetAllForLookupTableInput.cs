using Abp.Application.Services.Dto;

namespace QiProcureDemo.ReferenceTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}