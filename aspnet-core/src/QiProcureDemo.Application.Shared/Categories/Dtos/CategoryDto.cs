
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Categories.Dtos
{
    public class CategoryDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public bool IsApproved { get; set; }

		public bool IsActive { get; set; }



    }
}