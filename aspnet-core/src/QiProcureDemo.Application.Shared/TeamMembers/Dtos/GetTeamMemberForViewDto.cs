using System;

namespace QiProcureDemo.TeamMembers.Dtos
{
    public class GetTeamMemberForViewDto
    {
		public TeamMemberDto TeamMember { get; set; }

		public string TeamName { get; set;}

		public string UserName { get; set;}

		public string SysRefTenantId { get; set;}

		public string SysStatusName { get; set;}

		public string ProfilePicture { get; set; }

		public string FullName { get; set; }

		public Guid? ProfilePictureId { get; set; }

		public string SelectedRoleName { get; set; }

		public string EmailAddress { get; set; }
	}
}