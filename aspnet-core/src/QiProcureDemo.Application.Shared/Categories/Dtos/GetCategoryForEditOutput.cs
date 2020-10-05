using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Categories.Dtos
{
    public class GetCategoryForEditOutput
    {
		public CreateOrEditCategoryDto Category { get; set; }


    }
}