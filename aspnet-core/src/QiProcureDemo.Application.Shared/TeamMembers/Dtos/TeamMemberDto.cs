
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.TeamMembers.Dtos
{
    public class TeamMemberDto : EntityDto
    {
		public string Remark { get; set; }

		public int ReportingTeamMemberId { get; set; }


		public int? TeamId { get; set; }

		public long? UserId { get; set; }

		public int? SysRefId { get; set; }

		public int? SysStatusId { get; set; }

		public int RoleOrderNumber { get; set; }

		public string SelectedTeamRoleName { get; set; }
		
	}
}