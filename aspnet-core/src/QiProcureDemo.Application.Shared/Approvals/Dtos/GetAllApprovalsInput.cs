using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Approvals.Dtos
{
    public class GetAllApprovalsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxRankNoFilter { get; set; }
		public int? MinRankNoFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }


		public string SysRefTenantIdFilter { get; set; }

		public string TeamNameFilter { get; set; }

		public string ProjectNameFilter { get; set; }

		public string AccountNameFilter { get; set; }

		public string UserNameFilter { get; set; }

		public string SysStatusNameFilter { get; set; }

		public string RefCodeFilter { get; set; }

	}
}