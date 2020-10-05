using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProductCategories.Dtos
{
    public class GetProductCategoryForEditOutput
    {
		public CreateOrEditProductCategoryDto ProductCategory { get; set; }

		public string ProductName { get; set;}

		public string CategoryName { get; set;}


    }
}