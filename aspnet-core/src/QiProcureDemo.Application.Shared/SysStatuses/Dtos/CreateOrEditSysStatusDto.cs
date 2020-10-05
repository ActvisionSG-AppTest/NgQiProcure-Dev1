
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.SysStatuses.Dtos
{
    public class CreateOrEditSysStatusDto : EntityDto<int?>
    {

		[Range(SysStatusConsts.MinCodeValue, SysStatusConsts.MaxCodeValue)]
		public int Code { get; set; }
		
		
		[Required]
		[StringLength(SysStatusConsts.MaxNameLength, MinimumLength = SysStatusConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(SysStatusConsts.MaxDescriptionLength, MinimumLength = SysStatusConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		 public int? SysRefId { get; set; }
		 
		 
    }
}