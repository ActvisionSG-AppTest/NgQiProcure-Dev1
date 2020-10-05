using QiProcureDemo.Teams;
using QiProcureDemo.SysStatuses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Accounts
{
	[Table("QP_Accounts")]
    public class Account : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(AccountConsts.MaxNameLength, MinimumLength = AccountConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(AccountConsts.MaxDescriptionLength, MinimumLength = AccountConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		public virtual bool IsPersonal { get; set; }
		
		public virtual bool IsActive { get; set; }
		
		[StringLength(AccountConsts.MaxRemarkLength, MinimumLength = AccountConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }
		
		[Required]
		[StringLength(AccountConsts.MaxCodeLength, MinimumLength = AccountConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(AccountConsts.MaxEmailLength, MinimumLength = AccountConsts.MinEmailLength)]
		public virtual string Email { get; set; }
		
		[StringLength(AccountConsts.MaxUserNameLength, MinimumLength = AccountConsts.MinUserNameLength)]
		public virtual string UserName { get; set; }
		
		[StringLength(AccountConsts.MaxPasswordLength, MinimumLength = AccountConsts.MinPasswordLength)]
		public virtual string Password { get; set; }
		

		public virtual int? TeamId { get; set; }
		
        [ForeignKey("TeamId")]
		public Team TeamFk { get; set; }
		
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
    }
}