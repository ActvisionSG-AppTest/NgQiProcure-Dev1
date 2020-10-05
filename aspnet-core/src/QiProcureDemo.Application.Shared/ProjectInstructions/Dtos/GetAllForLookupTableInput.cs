using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProjectInstructions.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}