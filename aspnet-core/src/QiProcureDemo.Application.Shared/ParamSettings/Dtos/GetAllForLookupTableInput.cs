using Abp.Application.Services.Dto;

namespace QiProcureDemo.ParamSettings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}