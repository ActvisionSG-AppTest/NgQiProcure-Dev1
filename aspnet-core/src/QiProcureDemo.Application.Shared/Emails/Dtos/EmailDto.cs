
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Emails.Dtos
{
    public class EmailDto : EntityDto
    {
		public int ReferenceId { get; set; }

		public string EmailFrom { get; set; }

		public string EmailTo { get; set; }

		public string EmailCC { get; set; }

		public string EmailBCC { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public DateTime RequestDate { get; set; }

		public DateTime SentDate { get; set; }


		 public int? SysRefId { get; set; }

		 		 public int? SysStatusId { get; set; }

		 
    }
}