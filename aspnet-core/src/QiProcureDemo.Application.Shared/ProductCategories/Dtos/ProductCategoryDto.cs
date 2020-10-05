
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProductCategories.Dtos
{
    public class ProductCategoryDto : EntityDto
    {

		 public int? ProductId { get; set; }

		 public int? CategoryId { get; set; }

		 
    }
}