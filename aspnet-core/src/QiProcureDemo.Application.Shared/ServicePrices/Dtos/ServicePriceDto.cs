
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ServicePrices.Dtos
{
    public class ServicePriceDto : EntityDto
    {
		public decimal Price { get; set; }

		public DateTime Validity { get; set; }


		 public int? ServiceId { get; set; }

		 
    }
}