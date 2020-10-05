using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Products.Dtos
{
    public class GetProductForEditOutput
    {
		public CreateOrEditProductDto Product { get; set; }

		public string CategoryName { get; set;}


    }
}