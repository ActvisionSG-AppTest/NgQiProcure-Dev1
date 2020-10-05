
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Products.Dtos
{
    public class ProductDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public double Stock { get; set; }

		public string Uom { get; set; }

		public bool IsApproved { get; set; }

		public bool IsActive { get; set; }

		public string Remark { get; set; }


		 public int? CategoryId { get; set; }

		 
    }
}