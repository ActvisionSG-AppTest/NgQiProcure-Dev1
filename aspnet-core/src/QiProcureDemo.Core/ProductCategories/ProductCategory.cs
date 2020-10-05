using QiProcureDemo.Products;
using QiProcureDemo.Categories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ProductCategories
{
	[Table("QP_ProductCategories")]
    public class ProductCategory : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			
		public virtual int? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
		public virtual int? CategoryId { get; set; }
		
        [ForeignKey("CategoryId")]
		public Category CategoryFk { get; set; }
		
    }
}