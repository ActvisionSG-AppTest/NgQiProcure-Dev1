using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Services.Dtos
{
    public class GetAllServicesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public double? MaxDurationFilter { get; set; }
		public double? MinDurationFilter { get; set; }

		public int IsApprovedFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public string RemarkFilter { get; set; }


		 public string CategoryNameFilter { get; set; }

		 		 public string SysRefRefCodeFilter { get; set; }

		 
    }
}