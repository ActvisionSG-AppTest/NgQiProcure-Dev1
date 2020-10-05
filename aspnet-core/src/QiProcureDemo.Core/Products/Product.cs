using QiProcureDemo.Categories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Products
{
	[Table("QP_Products")]
    public class Product : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		[Range(ProductConsts.MinStockValue, ProductConsts.MaxStockValue)]
		public virtual double Stock { get; set; }
		
		[StringLength(ProductConsts.MaxUomLength, MinimumLength = ProductConsts.MinUomLength)]
		public virtual string Uom { get; set; }
		
		public virtual bool IsApproved { get; set; }
		
		public virtual bool IsActive { get; set; }
		
		[StringLength(ProductConsts.MaxRemarkLength, MinimumLength = ProductConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }
		

		public virtual int? CategoryId { get; set; }
		
        [ForeignKey("CategoryId")]
		public Category CategoryFk { get; set; }
		
    }
}