
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ServiceCategories.Dtos
{
    public class ServiceCategoryDto : EntityDto
    {

		 public int? ServiceId { get; set; }

		 		 public int? CategoryId { get; set; }

		 
    }
}