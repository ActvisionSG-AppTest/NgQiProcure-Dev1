
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Approvals.Dtos
{
    public class CreateOrEditApprovalDto : EntityDto<int?>
    {

		[Range(ApprovalConsts.MinRankNoValue, ApprovalConsts.MaxRankNoValue)]
		public int RankNo { get; set; }
		
		
		[Range(ApprovalConsts.MinAmountValue, ApprovalConsts.MaxAmountValue)]
		public decimal Amount { get; set; }
		
		
		 public int? SysRefId { get; set; }
		 
		 		 public int? TeamId { get; set; }
		 
		 		 public int? ProjectId { get; set; }
		 
		 		 public int? AccountId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 		 public int? SysStatusId { get; set; }
		 
		 
    }
}