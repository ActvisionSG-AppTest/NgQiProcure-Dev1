
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ApprovalRequests.Dtos
{
    public class ApprovalRequestDto : EntityDto
    {
		public int ReferenceId { get; set; }

		public int OrderNo { get; set; }

		public int RankNo { get; set; }

		public decimal Amount { get; set; }

		public string Remark { get; set; }


		 public int? SysRefId { get; set; }

		 		 public int? SysStatusId { get; set; }

		 		 public long? UserId { get; set; }
		public string SysStatusName { get; set; }


	}
}