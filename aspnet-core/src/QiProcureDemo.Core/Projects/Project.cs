using QiProcureDemo.Accounts;
using QiProcureDemo.Teams;
using QiProcureDemo.SysStatuses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Projects
{
	[Table("QP_Projects")]
    public class Project : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ProjectConsts.MaxNameLength, MinimumLength = ProjectConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(ProjectConsts.MaxDescriptionLength, MinimumLength = ProjectConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		public virtual DateTime? StartDate { get; set; }
		
		public virtual DateTime? EndDate { get; set; }
		
		public virtual bool IsApprove { get; set; }
		
		public virtual bool IsActive { get; set; }
		
		public virtual bool Publish { get; set; }
		
		[StringLength(ProjectConsts.MaxRemarkLength, MinimumLength = ProjectConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }
		

		public virtual int? AccountId { get; set; }
		
        [ForeignKey("AccountId")]
		public Account AccountFk { get; set; }
		
		public virtual int? TeamId { get; set; }
		
        [ForeignKey("TeamId")]
		public Team TeamFk { get; set; }
		
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
    }
}