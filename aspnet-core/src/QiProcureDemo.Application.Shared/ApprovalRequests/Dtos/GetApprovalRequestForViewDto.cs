using System;

namespace QiProcureDemo.ApprovalRequests.Dtos
{
    public class GetApprovalRequestForViewDto
    {
		public ApprovalRequestDto ApprovalRequest { get; set; }

		public string SysRefTenantId { get; set;}

		public string SysStatusName { get; set;}

		public string UserName { get; set;}

		public string EmailAddress { get; set; }

		public Guid? ProfilePictureId { get; set; }

		public string ProfilePicture { get; set; }

		public string FullName { get; set; }

	}
}