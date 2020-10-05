using QiProcureDemo.SysRefs;
using QiProcureDemo.SysStatuses;
using QiProcureDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ApprovalRequests
{
	[Table("QP_ApprovalRequests")]
    public class ApprovalRequest : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(ApprovalRequestConsts.MinReferenceIdValue, ApprovalRequestConsts.MaxReferenceIdValue)]
		public virtual int ReferenceId { get; set; }
		
		[Range(ApprovalRequestConsts.MinOrderNoValue, ApprovalRequestConsts.MaxOrderNoValue)]
		public virtual int OrderNo { get; set; }
		
		public virtual int RankNo { get; set; }
		
		[Range(ApprovalRequestConsts.MinAmountValue, ApprovalRequestConsts.MaxAmountValue)]
		public virtual decimal Amount { get; set; }
		
		[StringLength(ApprovalRequestConsts.MaxRemarkLength, MinimumLength = ApprovalRequestConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }
		

		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}