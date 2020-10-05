using QiProcureDemo.Services;
using QiProcureDemo.Categories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ServiceCategories
{
	[Table("QP_ServiceCategories")]
    public class ServiceCategory : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			


		public virtual int? ServiceId { get; set; }
		
        [ForeignKey("ServiceId")]
		public Service ServiceFk { get; set; }
		
		public virtual int? CategoryId { get; set; }
		
        [ForeignKey("CategoryId")]
		public Category CategoryFk { get; set; }
		
    }
}