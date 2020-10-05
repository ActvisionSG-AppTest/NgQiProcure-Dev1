using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ParamSettings
{
	[Table("QP_ParamSettings")]
    public class ParamSetting : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ParamSettingConsts.MaxNameLength, MinimumLength = ParamSettingConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(ParamSettingConsts.MaxValueLength, MinimumLength = ParamSettingConsts.MinValueLength)]
		public virtual string Value { get; set; }
		
		[StringLength(ParamSettingConsts.MaxDescriptionLength, MinimumLength = ParamSettingConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		

    }
}