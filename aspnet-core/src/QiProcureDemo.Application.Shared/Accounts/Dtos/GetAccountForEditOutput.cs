using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Accounts.Dtos
{
    public class GetAccountForEditOutput
    {
		public CreateOrEditAccountDto Account { get; set; }

		public string TeamName { get; set;}

		public string SysStatusName { get; set;}


    }
}