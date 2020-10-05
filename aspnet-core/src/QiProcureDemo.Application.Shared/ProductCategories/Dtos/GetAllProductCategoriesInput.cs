using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ProductCategories.Dtos
{
    public class GetAllProductCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string ProductNameFilter { get; set; }

		 		 public string CategoryNameFilter { get; set; }

		 
    }
}