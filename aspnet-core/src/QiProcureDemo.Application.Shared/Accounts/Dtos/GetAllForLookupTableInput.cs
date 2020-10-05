using Abp.Application.Services.Dto;

namespace QiProcureDemo.Accounts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}