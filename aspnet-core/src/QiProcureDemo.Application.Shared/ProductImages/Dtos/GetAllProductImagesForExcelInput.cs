using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ProductImages.Dtos
{
    public class GetAllProductImagesForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public string UrlFilter { get; set; }

		public int IsMainFilter { get; set; }

		public int IsApprovedFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}