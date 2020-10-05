
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.SysStatuses.Dtos
{
    public class SysStatusDto : EntityDto
    {
		public int Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }


		 public int? SysRefId { get; set; }

		public string RefCode { get; set; }


	}
}