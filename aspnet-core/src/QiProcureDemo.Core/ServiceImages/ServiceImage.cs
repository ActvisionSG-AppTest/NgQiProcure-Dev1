using QiProcureDemo.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ServiceImages
{
	[Table("QP_ServiceImages")]
    public class ServiceImage : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(ServiceImageConsts.MaxDescriptionLength, MinimumLength = ServiceImageConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		[StringLength(ServiceImageConsts.MaxUrlLength, MinimumLength = ServiceImageConsts.MinUrlLength)]
		public virtual string Url { get; set; }
		
		public virtual bool IsMain { get; set; }
		
		public virtual bool IsApproved { get; set; }
		
		public virtual byte[] Bytes { get; set; }
		

		public virtual int? ServiceId { get; set; }
		
        [ForeignKey("ServiceId")]
		public Service ServiceFk { get; set; }
		
    }
}