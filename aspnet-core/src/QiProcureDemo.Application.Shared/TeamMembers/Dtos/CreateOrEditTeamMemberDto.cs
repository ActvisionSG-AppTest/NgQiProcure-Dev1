
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.TeamMembers.Dtos
{
    public class CreateOrEditTeamMemberDto : EntityDto<int?>
    {

		[StringLength(TeamMemberConsts.MaxRemarkLength, MinimumLength = TeamMemberConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		public int ReportingTeamMemberId { get; set; }
		
		
		 public int? TeamId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 		 public int? SysRefId { get; set; }
		 
		 		 public int? SysStatusId { get; set; }
		 
		 
    }
}