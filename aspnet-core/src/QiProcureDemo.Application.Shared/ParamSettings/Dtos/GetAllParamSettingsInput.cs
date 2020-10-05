using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ParamSettings.Dtos
{
    public class GetAllParamSettingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string ValueFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}