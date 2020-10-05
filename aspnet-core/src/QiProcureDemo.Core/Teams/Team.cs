using QiProcureDemo.SysStatuses;
using QiProcureDemo.ReferenceTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Teams
{
	[Table("QP_Teams")]
    public class Team : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(TeamConsts.MaxNameLength, MinimumLength = TeamConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(TeamConsts.MaxDescriptionLength, MinimumLength = TeamConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		public virtual bool IsActive { get; set; }
		
		[StringLength(TeamConsts.MaxRemarkLength, MinimumLength = TeamConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }

/*		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]*/
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
		public virtual int? ReferenceTypeId { get; set; }
		
        [ForeignKey("ReferenceTypeId")]
		public ReferenceType ReferenceTypeFk { get; set; }
		
    }
}