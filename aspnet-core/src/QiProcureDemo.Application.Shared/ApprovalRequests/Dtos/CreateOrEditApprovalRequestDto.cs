
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ApprovalRequests.Dtos
{
    public class CreateOrEditApprovalRequestDto : EntityDto<int?>
    {

		[Range(ApprovalRequestConsts.MinReferenceIdValue, ApprovalRequestConsts.MaxReferenceIdValue)]
		public int ReferenceId { get; set; }
		
		
		[Range(ApprovalRequestConsts.MinOrderNoValue, ApprovalRequestConsts.MaxOrderNoValue)]
		public int OrderNo { get; set; }
		
		
		public int RankNo { get; set; }
		
		
		[Range(ApprovalRequestConsts.MinAmountValue, ApprovalRequestConsts.MaxAmountValue)]
		public decimal Amount { get; set; }
		
		
		[StringLength(ApprovalRequestConsts.MaxRemarkLength, MinimumLength = ApprovalRequestConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		 public int? SysRefId { get; set; }
		 
		 		 public int? SysStatusId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}