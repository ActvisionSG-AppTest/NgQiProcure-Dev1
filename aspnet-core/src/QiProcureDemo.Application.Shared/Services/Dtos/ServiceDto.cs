
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Services.Dtos
{
    public class ServiceDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public double Duration { get; set; }

		public bool IsApproved { get; set; }

		public bool IsActive { get; set; }

		public string Remark { get; set; }


		 public int? CategoryId { get; set; }

		 		 public int? SysRefId { get; set; }

		 
    }
}