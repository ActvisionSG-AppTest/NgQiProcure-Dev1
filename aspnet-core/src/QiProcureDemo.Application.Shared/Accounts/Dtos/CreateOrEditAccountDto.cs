
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Accounts.Dtos
{
    public class CreateOrEditAccountDto : EntityDto<int?>
    {

		[Required]
		[StringLength(AccountConsts.MaxNameLength, MinimumLength = AccountConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(AccountConsts.MaxDescriptionLength, MinimumLength = AccountConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		public bool IsPersonal { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		[StringLength(AccountConsts.MaxRemarkLength, MinimumLength = AccountConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		[Required]
		[StringLength(AccountConsts.MaxCodeLength, MinimumLength = AccountConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(AccountConsts.MaxEmailLength, MinimumLength = AccountConsts.MinEmailLength)]
		public string Email { get; set; }
		
		
		[StringLength(AccountConsts.MaxUserNameLength, MinimumLength = AccountConsts.MinUserNameLength)]
		public string UserName { get; set; }
		
		
		[StringLength(AccountConsts.MaxPasswordLength, MinimumLength = AccountConsts.MinPasswordLength)]
		public string Password { get; set; }
		
		
		 public int? TeamId { get; set; }
		 
		 		 public int? SysStatusId { get; set; }
		 
		 
    }
}