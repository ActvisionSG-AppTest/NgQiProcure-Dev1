
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ServicePrices.Dtos
{
    public class CreateOrEditServicePriceDto : EntityDto<int?>
    {

		[Range(ServicePriceConsts.MinPriceValue, ServicePriceConsts.MaxPriceValue)]
		public decimal Price { get; set; }
		
		
		public DateTime Validity { get; set; }
		
		
		 public int? ServiceId { get; set; }
		 
		 
    }
}