
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProductPrices.Dtos
{
    public class CreateOrEditProductPriceDto : EntityDto<int?>
    {

		[Range(ProductPriceConsts.MinPriceValue, ProductPriceConsts.MaxPriceValue)]
		public decimal Price { get; set; }
		
		
		public DateTime validity { get; set; }
		
		
		 public int? ProductId { get; set; }
		 
		 
    }
}