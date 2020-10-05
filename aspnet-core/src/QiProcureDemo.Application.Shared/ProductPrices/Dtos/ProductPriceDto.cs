
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProductPrices.Dtos
{
    public class ProductPriceDto : EntityDto
    {
		public decimal Price { get; set; }

		public DateTime validity { get; set; }


		 public int? ProductId { get; set; }

		 
    }
}