using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ApprovalRequests.Dtos.Custom
{
    public class GetCustomApprovalRequestsInput
    {
		
		public int? ReferenceId { get; set; }
		public int? SysRefId { get; set; }
		public int? SysStatusId { get; set; }

       /* public GetCustomApprovalRequestsOutput(RequestOutput, List<GetApprovalRequestForViewDto> approvalRequests)
           : base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }*/
    }
}