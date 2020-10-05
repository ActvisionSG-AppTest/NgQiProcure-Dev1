
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProductImages.Dtos
{
    public class CreateOrEditProductImageDto : EntityDto<int?>
    {

		[StringLength(ProductImageConsts.MaxDescriptionLength, MinimumLength = ProductImageConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		[StringLength(ProductImageConsts.MaxUrlLength, MinimumLength = ProductImageConsts.MinUrlLength)]
		public string Url { get; set; }
		
		
		public bool IsMain { get; set; }
		
		
		public bool IsApproved { get; set; }
		
		
		 public int? ProductId { get; set; }

		public byte[] Bytes { get; set; }


	}
}