
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Accounts.Dtos
{
    public class AccountDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public bool IsPersonal { get; set; }

		public bool IsActive { get; set; }

		public string Remark { get; set; }

		public string Code { get; set; }

		public string Email { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }


		 public int? TeamId { get; set; }

		 		 public int? SysStatusId { get; set; }

		 
    }
}