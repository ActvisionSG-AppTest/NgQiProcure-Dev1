
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Emails.Dtos
{
    public class CreateOrEditEmailDto : EntityDto<int?>
    {

		[Range(EmailConsts.MinReferenceIdValue, EmailConsts.MaxReferenceIdValue)]
		public int ReferenceId { get; set; }
		
		
		[Required]
		[StringLength(EmailConsts.MaxEmailFromLength, MinimumLength = EmailConsts.MinEmailFromLength)]
		public string EmailFrom { get; set; }
		
		
		[Required]
		[StringLength(EmailConsts.MaxEmailToLength, MinimumLength = EmailConsts.MinEmailToLength)]
		public string EmailTo { get; set; }
		
		
		[StringLength(EmailConsts.MaxEmailCCLength, MinimumLength = EmailConsts.MinEmailCCLength)]
		public string EmailCC { get; set; }
		
		
		[StringLength(EmailConsts.MaxEmailBCCLength, MinimumLength = EmailConsts.MinEmailBCCLength)]
		public string EmailBCC { get; set; }
		
		
		[Required]
		[StringLength(EmailConsts.MaxSubjectLength, MinimumLength = EmailConsts.MinSubjectLength)]
		public string Subject { get; set; }
		
		
		[Required]
		[StringLength(EmailConsts.MaxBodyLength, MinimumLength = EmailConsts.MinBodyLength)]
		public string Body { get; set; }
		
		
		public DateTime RequestDate { get; set; }
		
		
		public DateTime SentDate { get; set; }
		
		
		 public int? SysRefId { get; set; }
		 
		 		 public int? SysStatusId { get; set; }
		 
		 
    }
}