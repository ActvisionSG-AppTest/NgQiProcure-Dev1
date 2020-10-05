using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Teams.Dtos
{
    public class GetAllTeamsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public string RemarkFilter { get; set; }


		 public string SysStatusNameFilter { get; set; }

		 		 public string ReferenceTypeNameFilter { get; set; }

		 
    }
}