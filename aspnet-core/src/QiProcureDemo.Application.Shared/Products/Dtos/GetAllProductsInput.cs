using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Products.Dtos
{
    public class GetAllProductsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public double? MaxStockFilter { get; set; }
		public double? MinStockFilter { get; set; }

		public string UomFilter { get; set; }

		public int IsApprovedFilter { get; set; }

		public int IsActiveFilter { get; set; }


		 public string CategoryNameFilter { get; set; }

		 
    }
}