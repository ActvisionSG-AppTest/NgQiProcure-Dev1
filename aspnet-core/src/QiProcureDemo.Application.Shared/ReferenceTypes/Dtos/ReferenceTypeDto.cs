
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ReferenceTypes.Dtos
{
    public class ReferenceTypeDto : EntityDto
    {
		public string Name { get; set; }

		public string ReferenceTypeCode { get; set; }

		public string ReferenceTypeGroup { get; set; }



    }
}