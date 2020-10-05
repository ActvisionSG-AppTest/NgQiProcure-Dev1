using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Categories
{
	[Table("QP_Categories")]
    public class Category : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(CategoryConsts.MaxNameLength, MinimumLength = CategoryConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(CategoryConsts.MaxDescriptionLength, MinimumLength = CategoryConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		public virtual bool IsApproved { get; set; }
		
		public virtual bool IsActive { get; set; }
		

    }
}