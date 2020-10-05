using Abp.Application.Services.Dto;

namespace QiProcureDemo.TeamMembers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}