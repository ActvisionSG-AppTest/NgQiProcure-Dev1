using Abp.Application.Services.Dto;

namespace QiProcureDemo.Projects.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}