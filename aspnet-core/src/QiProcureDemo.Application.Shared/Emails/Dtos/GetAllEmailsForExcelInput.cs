using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Emails.Dtos
{
    public class GetAllEmailsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxReferenceIdFilter { get; set; }
		public int? MinReferenceIdFilter { get; set; }

		public string EmailFromFilter { get; set; }

		public string EmailToFilter { get; set; }

		public string EmailCCFilter { get; set; }

		public string EmailBCCFilter { get; set; }

		public string SubjectFilter { get; set; }

		public string BodyFilter { get; set; }

		public DateTime? MaxRequestDateFilter { get; set; }
		public DateTime? MinRequestDateFilter { get; set; }

		public DateTime? MaxSentDateFilter { get; set; }
		public DateTime? MinSentDateFilter { get; set; }


		 public string SysRefTenantIdFilter { get; set; }

		 		 public string SysStatusNameFilter { get; set; }

		 
    }
}