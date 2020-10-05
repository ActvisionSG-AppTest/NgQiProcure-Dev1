
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Services.Dtos
{
    public class CreateOrEditServiceDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ServiceConsts.MaxNameLength, MinimumLength = ServiceConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(ServiceConsts.MaxDescriptionLength, MinimumLength = ServiceConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		[Range(ServiceConsts.MinDurationValue, ServiceConsts.MaxDurationValue)]
		public double Duration { get; set; }
		
		
		public bool IsApproved { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		[StringLength(ServiceConsts.MaxRemarkLength, MinimumLength = ServiceConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		 public int? CategoryId { get; set; }
		 
		 		 public int? SysRefId { get; set; }
		 
		 
    }
}