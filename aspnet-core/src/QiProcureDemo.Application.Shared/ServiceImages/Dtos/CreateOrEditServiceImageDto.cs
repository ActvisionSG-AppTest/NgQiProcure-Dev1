
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ServiceImages.Dtos
{
    public class CreateOrEditServiceImageDto : EntityDto<int?>
    {

		[StringLength(ServiceImageConsts.MaxDescriptionLength, MinimumLength = ServiceImageConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		[StringLength(ServiceImageConsts.MaxUrlLength, MinimumLength = ServiceImageConsts.MinUrlLength)]
		public string Url { get; set; }
		
		
		public bool IsMain { get; set; }
		
		
		public string IsApproved { get; set; }
		
		
		public byte[] Bytes { get; set; }
		
		
		 public int? ServiceId { get; set; }
		 
		 
    }
}