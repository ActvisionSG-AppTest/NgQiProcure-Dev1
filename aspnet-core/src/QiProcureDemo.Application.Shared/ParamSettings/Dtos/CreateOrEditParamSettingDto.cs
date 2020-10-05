
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ParamSettings.Dtos
{
    public class CreateOrEditParamSettingDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ParamSettingConsts.MaxNameLength, MinimumLength = ParamSettingConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(ParamSettingConsts.MaxValueLength, MinimumLength = ParamSettingConsts.MinValueLength)]
		public string Value { get; set; }
		
		
		[StringLength(ParamSettingConsts.MaxDescriptionLength, MinimumLength = ParamSettingConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		

    }
}