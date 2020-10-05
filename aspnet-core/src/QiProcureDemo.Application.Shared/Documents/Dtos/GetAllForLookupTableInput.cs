using Abp.Application.Services.Dto;

namespace QiProcureDemo.Documents.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}