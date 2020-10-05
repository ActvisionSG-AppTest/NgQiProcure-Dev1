using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Emails.Dtos
{
    public class GetEmailForEditOutput
    {
		public CreateOrEditEmailDto Email { get; set; }

		public string SysRefTenantId { get; set;}

		public string SysStatusName { get; set;}


    }
}