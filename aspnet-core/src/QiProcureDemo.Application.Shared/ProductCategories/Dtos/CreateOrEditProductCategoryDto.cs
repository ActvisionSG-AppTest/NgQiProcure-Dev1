
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProductCategories.Dtos
{
    public class CreateOrEditProductCategoryDto : EntityDto<int?>
    {

		 public int? ProductId { get; set; }
		 
		 		 public int? CategoryId { get; set; }
		 
		 
    }
}