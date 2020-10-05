
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ReferenceTypes.Dtos
{
    public class CreateOrEditReferenceTypeDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ReferenceTypeConsts.MaxNameLength, MinimumLength = ReferenceTypeConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(ReferenceTypeConsts.MaxReferenceTypeCodeLength, MinimumLength = ReferenceTypeConsts.MinReferenceTypeCodeLength)]
		public string ReferenceTypeCode { get; set; }
		
		
		[StringLength(ReferenceTypeConsts.MaxReferenceTypeGroupLength, MinimumLength = ReferenceTypeConsts.MinReferenceTypeGroupLength)]
		public string ReferenceTypeGroup { get; set; }
		
		

    }
}