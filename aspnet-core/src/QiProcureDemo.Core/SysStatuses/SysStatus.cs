using QiProcureDemo.SysRefs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.SysStatuses
{
	[Table("QP_SysStatuses")]
    public class SysStatus : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(SysStatusConsts.MinCodeValue, SysStatusConsts.MaxCodeValue)]
		public virtual int Code { get; set; }
		
		[Required]
		[StringLength(SysStatusConsts.MaxNameLength, MinimumLength = SysStatusConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(SysStatusConsts.MaxDescriptionLength, MinimumLength = SysStatusConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		

		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
    }
}