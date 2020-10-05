
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ParamSettings.Dtos
{
    public class ParamSettingDto : EntityDto
    {
		public string Name { get; set; }

		public string Value { get; set; }

		public string Description { get; set; }



    }
}