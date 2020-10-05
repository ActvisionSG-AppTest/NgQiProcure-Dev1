namespace QiProcureDemo.ApprovalRequests.Dtos.Custom
{
    public class GetCustomApprovalRequestForViewDto
    {
        public ApprovalRequestDto ApprovalRequest { get; set; }

        public string SysRefTenantId { get; set; }

        public string SysStatusName { get; set; }

    }
}