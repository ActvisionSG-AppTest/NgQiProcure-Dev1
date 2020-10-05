
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Categories.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<int?>
    {

		[Required]
		[StringLength(CategoryConsts.MaxNameLength, MinimumLength = CategoryConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(CategoryConsts.MaxDescriptionLength, MinimumLength = CategoryConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		public bool IsApproved { get; set; }
		
		
		public bool IsActive { get; set; }
		
		

    }
}