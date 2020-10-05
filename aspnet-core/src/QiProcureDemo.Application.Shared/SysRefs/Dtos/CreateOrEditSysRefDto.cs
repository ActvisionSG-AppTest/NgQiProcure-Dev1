
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.SysRefs.Dtos
{
    public class CreateOrEditSysRefDto : EntityDto<int?>
    {

		[Required]
		[StringLength(SysRefConsts.MaxRefCodeLength, MinimumLength = SysRefConsts.MinRefCodeLength)]
		public string RefCode { get; set; }
		
		
		[StringLength(SysRefConsts.MaxDescriptionLength, MinimumLength = SysRefConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		public int OrderNumber { get; set; }
		
		
		 public int? ReferenceTypeId { get; set; }
		 
		 
    }
}