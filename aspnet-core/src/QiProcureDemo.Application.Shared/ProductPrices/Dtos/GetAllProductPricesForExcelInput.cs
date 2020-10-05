using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ProductPrices.Dtos
{
    public class GetAllProductPricesForExcelInput
    {
		public string Filter { get; set; }

		public decimal? MaxPriceFilter { get; set; }
		public decimal? MinPriceFilter { get; set; }

		public DateTime? MaxvalidityFilter { get; set; }
		public DateTime? MinvalidityFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}