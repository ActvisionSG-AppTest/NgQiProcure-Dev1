using QiProcureDemo.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace QiProcureDemo.ProductPrices
{
	[Table("QP_ProductPrices")]
    [Audited]
    public class ProductPrice : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(ProductPriceConsts.MinPriceValue, ProductPriceConsts.MaxPriceValue)]
		public virtual decimal Price { get; set; }
		
		public virtual DateTime validity { get; set; }
		

		public virtual int? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}