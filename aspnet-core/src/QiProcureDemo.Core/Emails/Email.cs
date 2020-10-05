using QiProcureDemo.SysRefs;
using QiProcureDemo.SysStatuses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.Emails
{
	[Table("QP_Emails")]
    public class Email : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(EmailConsts.MinReferenceIdValue, EmailConsts.MaxReferenceIdValue)]
		public virtual int ReferenceId { get; set; }
		
		[Required]
		[StringLength(EmailConsts.MaxEmailFromLength, MinimumLength = EmailConsts.MinEmailFromLength)]
		public virtual string EmailFrom { get; set; }
		
		[Required]
		[StringLength(EmailConsts.MaxEmailToLength, MinimumLength = EmailConsts.MinEmailToLength)]
		public virtual string EmailTo { get; set; }
		
		[StringLength(EmailConsts.MaxEmailCCLength, MinimumLength = EmailConsts.MinEmailCCLength)]
		public virtual string EmailCC { get; set; }
		
		[StringLength(EmailConsts.MaxEmailBCCLength, MinimumLength = EmailConsts.MinEmailBCCLength)]
		public virtual string EmailBCC { get; set; }
		
		[Required]
		[StringLength(EmailConsts.MaxSubjectLength, MinimumLength = EmailConsts.MinSubjectLength)]
		public virtual string Subject { get; set; }
		
		[Required]
		[StringLength(EmailConsts.MaxBodyLength, MinimumLength = EmailConsts.MinBodyLength)]
		public virtual string Body { get; set; }
		
		public virtual DateTime RequestDate { get; set; }
		
		public virtual DateTime SentDate { get; set; }
		

		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
    }
}