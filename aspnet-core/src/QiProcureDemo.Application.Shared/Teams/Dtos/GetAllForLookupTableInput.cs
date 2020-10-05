using Abp.Application.Services.Dto;

namespace QiProcureDemo.Teams.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public string ReferenceTypeGroupFilter { get; set; }

    }
}