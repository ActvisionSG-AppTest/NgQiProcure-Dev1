using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.SysRefs.Dtos.Custom
{
    public class GetAllSysRefsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string RefCodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int? MaxOrderNumberFilter { get; set; }
		public int? MinOrderNumberFilter { get; set; }


		public string ReferenceTypeNameFilter { get; set; }

		public int? ReferenceTypeIdFilter { get; set; }


	}
}