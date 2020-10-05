using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.SysStatuses.Dtos
{
    public class GetAllSysStatusesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxCodeFilter { get; set; }
		public int? MinCodeFilter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string SysRefTenantIdFilter { get; set; }

		public string RefCodeFilter { get; set; }


	}
}