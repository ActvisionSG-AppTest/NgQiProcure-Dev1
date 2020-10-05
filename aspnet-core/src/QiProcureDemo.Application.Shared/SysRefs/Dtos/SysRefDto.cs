
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.SysRefs.Dtos
{
    public class SysRefDto : EntityDto
    {
		public string RefCode { get; set; }

		public string Description { get; set; }

		public int OrderNumber { get; set; }


		 public int? ReferenceTypeId { get; set; }

		 
    }
}