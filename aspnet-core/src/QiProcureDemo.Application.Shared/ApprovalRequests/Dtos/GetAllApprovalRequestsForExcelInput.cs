using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ApprovalRequests.Dtos
{
    public class GetAllApprovalRequestsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxReferenceIdFilter { get; set; }
		public int? MinReferenceIdFilter { get; set; }

		public int? MaxOrderNoFilter { get; set; }
		public int? MinOrderNoFilter { get; set; }

		public int? MaxRankNoFilter { get; set; }
		public int? MinRankNoFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }

		public string RemarkFilter { get; set; }


		 public string SysRefTenantIdFilter { get; set; }

		 		 public string SysStatusNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}