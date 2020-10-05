using QiProcureDemo.Categories;
using QiProcureDemo.SysRefs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Services
{
	[Table("QP_Services")]
    public class Service : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ServiceConsts.MaxNameLength, MinimumLength = ServiceConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(ServiceConsts.MaxDescriptionLength, MinimumLength = ServiceConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		[Range(ServiceConsts.MinDurationValue, ServiceConsts.MaxDurationValue)]
		public virtual double Duration { get; set; }
		
		public virtual bool IsApproved { get; set; }
		
		public virtual bool IsActive { get; set; }
		
		[StringLength(ServiceConsts.MaxRemarkLength, MinimumLength = ServiceConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }
		

		public virtual int? CategoryId { get; set; }
		
        [ForeignKey("CategoryId")]
		public Category CategoryFk { get; set; }
		
		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
    }
}