using QiProcureDemo.ReferenceTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.SysRefs
{
	[Table("QP_SysRefs")]
    public class SysRef : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(SysRefConsts.MaxRefCodeLength, MinimumLength = SysRefConsts.MinRefCodeLength)]
		public virtual string RefCode { get; set; }
		
		[StringLength(SysRefConsts.MaxDescriptionLength, MinimumLength = SysRefConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		public virtual int OrderNumber { get; set; }
		

		public virtual int? ReferenceTypeId { get; set; }
		
        [ForeignKey("ReferenceTypeId")]
		public ReferenceType ReferenceTypeFk { get; set; }
		
    }
}