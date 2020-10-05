namespace QiProcureDemo.Approvals.Dtos
{
    public class GetApprovalForViewDto
    {
		public ApprovalDto Approval { get; set; }

		public string SysRefTenantId { get; set;}

		public string TeamName { get; set;}

		public string ProjectName { get; set;}

		public string AccountName { get; set;}

		public string UserName { get; set;}

		public string SysStatusName { get; set;}

		public string RefCode { get; set; }


	}
}