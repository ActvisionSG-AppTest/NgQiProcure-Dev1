using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Categories.Dtos
{
    public class GetAllCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int IsApprovedFilter { get; set; }

		public int IsActiveFilter { get; set; }



    }
}