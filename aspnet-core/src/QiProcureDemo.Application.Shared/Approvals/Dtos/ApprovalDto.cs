
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Approvals.Dtos
{
    public class ApprovalDto : EntityDto
    {
		public int RankNo { get; set; }

		public decimal Amount { get; set; }


		 public int? SysRefId { get; set; }

		 		 public int? TeamId { get; set; }

		 		 public int? ProjectId { get; set; }

		 		 public int? AccountId { get; set; }

		 		 public long? UserId { get; set; }

		 		 public int? SysStatusId { get; set; }

		public string ReferenceNo { get; set; }

	}
}