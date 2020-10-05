using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ServicePrices.Dtos
{
    public class GetAllServicePricesForExcelInput
    {
		public string Filter { get; set; }

		public decimal? MaxPriceFilter { get; set; }
		public decimal? MinPriceFilter { get; set; }

		public DateTime? MaxValidityFilter { get; set; }
		public DateTime? MinValidityFilter { get; set; }


		 public string ServiceNameFilter { get; set; }

		 
    }
}