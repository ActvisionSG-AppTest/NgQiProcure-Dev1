using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ParamSettings.Dtos
{
    public class GetAllParamSettingsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string ValueFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}