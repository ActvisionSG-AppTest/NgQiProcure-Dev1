using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Accounts.Dtos
{
    public class GetAllAccountsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int IsPersonalFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public string RemarkFilter { get; set; }

		public string CodeFilter { get; set; }

		public string EmailFilter { get; set; }

		public string UserNameFilter { get; set; }

		public string PasswordFilter { get; set; }


		 public string TeamNameFilter { get; set; }

		 		 public string SysStatusNameFilter { get; set; }

		 
    }
}