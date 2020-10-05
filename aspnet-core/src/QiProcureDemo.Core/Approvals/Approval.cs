using QiProcureDemo.SysRefs;
using QiProcureDemo.Teams;
using QiProcureDemo.Projects;
using QiProcureDemo.Accounts;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.SysStatuses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Approvals
{
	[Table("QP_Approvals")]
    public class Approval : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(ApprovalConsts.MinRankNoValue, ApprovalConsts.MaxRankNoValue)]
		public virtual int RankNo { get; set; }
		
		[Range(ApprovalConsts.MinAmountValue, ApprovalConsts.MaxAmountValue)]
		public virtual decimal Amount { get; set; }
		

		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
		public virtual int? TeamId { get; set; }
		
        [ForeignKey("TeamId")]
		public Team TeamFk { get; set; }
		
		public virtual int? ProjectId { get; set; }
		
        [ForeignKey("ProjectId")]
		public Project ProjectFk { get; set; }
		
		public virtual int? AccountId { get; set; }
		
        [ForeignKey("AccountId")]
		public Account AccountFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
    }
}