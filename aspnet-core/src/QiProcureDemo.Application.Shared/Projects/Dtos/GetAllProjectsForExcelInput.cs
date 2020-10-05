using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Projects.Dtos
{
    public class GetAllProjectsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }

		public int IsApproveFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public int PublishFilter { get; set; }

		public string RemarkFilter { get; set; }


		 public string AccountNameFilter { get; set; }

		 		 public string TeamNameFilter { get; set; }

		 		 public string SysStatusNameFilter { get; set; }

		 
    }
}