using QiProcureDemo.SysRefs;
using QiProcureDemo.Products;
using QiProcureDemo.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Documents
{
	[Table("QP_Documents")]
    public class Document : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(DocumentConsts.MaxUrlLength, MinimumLength = DocumentConsts.MinUrlLength)]
		public virtual string Url { get; set; }
		
		[Required]
		[StringLength(DocumentConsts.MaxNameLength, MinimumLength = DocumentConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(DocumentConsts.MaxDescriptionLength, MinimumLength = DocumentConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		

		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
		public virtual int? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
		public virtual int? ServiceId { get; set; }
		
        [ForeignKey("ServiceId")]
		public Service ServiceFk { get; set; }

		public virtual byte[] Bytes { get; set; }


	}
}