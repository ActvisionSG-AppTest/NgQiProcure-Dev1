using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ServiceCategories.Dtos
{
    public class GetAllServiceCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string ServiceNameFilter { get; set; }

		 		 public string CategoryNameFilter { get; set; }

		 
    }
}