
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Products.Dtos
{
    public class CreateOrEditProductDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		[Range(ProductConsts.MinStockValue, ProductConsts.MaxStockValue)]
		public double Stock { get; set; }
		
		
		[StringLength(ProductConsts.MaxUomLength, MinimumLength = ProductConsts.MinUomLength)]
		public string Uom { get; set; }
		
		
		public bool IsApproved { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		[StringLength(ProductConsts.MaxRemarkLength, MinimumLength = ProductConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		 public int? CategoryId { get; set; }
		 
		 
    }
}