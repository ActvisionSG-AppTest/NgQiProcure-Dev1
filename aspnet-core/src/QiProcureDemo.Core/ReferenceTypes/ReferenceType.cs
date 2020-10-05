using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ReferenceTypes
{
	[Table("QP_ReferenceTypes")]
    public class ReferenceType : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ReferenceTypeConsts.MaxNameLength, MinimumLength = ReferenceTypeConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(ReferenceTypeConsts.MaxReferenceTypeCodeLength, MinimumLength = ReferenceTypeConsts.MinReferenceTypeCodeLength)]
		public virtual string ReferenceTypeCode { get; set; }
		
		[StringLength(ReferenceTypeConsts.MaxReferenceTypeGroupLength, MinimumLength = ReferenceTypeConsts.MinReferenceTypeGroupLength)]
		public virtual string ReferenceTypeGroup { get; set; }
		

    }
}