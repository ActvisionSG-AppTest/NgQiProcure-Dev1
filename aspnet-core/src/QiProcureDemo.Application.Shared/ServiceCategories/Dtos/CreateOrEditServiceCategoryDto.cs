
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ServiceCategories.Dtos
{
    public class CreateOrEditServiceCategoryDto : EntityDto<int?>
    {

		 public int? ServiceId { get; set; }
		 
		 		 public int? CategoryId { get; set; }
		 
		 
    }
}