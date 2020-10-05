using QiProcureDemo.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ServicePrices
{
	[Table("QP_ServicePrices")]
    public class ServicePrice : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(ServicePriceConsts.MinPriceValue, ServicePriceConsts.MaxPriceValue)]
		public virtual decimal Price { get; set; }
		
		public virtual DateTime Validity { get; set; }
		

		public virtual int? ServiceId { get; set; }
		
        [ForeignKey("ServiceId")]
		public Service ServiceFk { get; set; }
		
    }
}