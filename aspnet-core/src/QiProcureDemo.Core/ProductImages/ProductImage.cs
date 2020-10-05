using QiProcureDemo.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ProductImages
{
	[Table("QP_ProductImages")]
    public class ProductImage : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(ProductImageConsts.MaxDescriptionLength, MinimumLength = ProductImageConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		[StringLength(ProductImageConsts.MaxUrlLength, MinimumLength = ProductImageConsts.MinUrlLength)]
		public virtual string Url { get; set; }
		
		public virtual bool IsMain { get; set; }
		
		public virtual bool IsApproved { get; set; }
		

		public virtual int? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }

		public byte[] Bytes { get; set; }

	}
}