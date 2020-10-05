using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.TeamMembers.Dtos
{
    public class GetAllTeamMembersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string RemarkFilter { get; set; }

		public int? MaxReportingTeamMemberIdFilter { get; set; }
		public int? MinReportingTeamMemberIdFilter { get; set; }


		public string TeamNameFilter { get; set; }

		public string UserNameFilter { get; set; }

		public string SysRefTenantIdFilter { get; set; }

		public string SysStatusNameFilter { get; set; }

		public int? ReportingTeamMemberIdFilter { get; set; }

		public int? TeamIdFilter { get; set; }

	}
}