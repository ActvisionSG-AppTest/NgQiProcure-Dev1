using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProductPrices.Dtos
{
    public class GetProductPriceForEditOutput
    {
		public CreateOrEditProductPriceDto ProductPrice { get; set; }

		public string ProductName { get; set;}


    }
}