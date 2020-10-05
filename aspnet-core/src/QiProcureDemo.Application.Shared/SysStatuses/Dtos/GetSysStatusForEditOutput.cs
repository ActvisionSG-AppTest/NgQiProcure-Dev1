using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.SysStatuses.Dtos
{
    public class GetSysStatusForEditOutput
    {
		public CreateOrEditSysStatusDto SysStatus { get; set; }

		public string SysRefTenantId { get; set;}


    }
}