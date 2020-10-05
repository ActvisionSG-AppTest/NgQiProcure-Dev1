using Abp.Application.Services.Dto;
using QiProcureDemo.Common.Dto.Custom;
using System;
using System.Collections.Generic;

namespace QiProcureDemo.ApprovalRequests.Dtos.Custom
{
    public class GetCustomApprovalRequestsOutput
    {

        public RequestOutput Result { get; set; }
        public List<GetCustomApprovalRequestForViewDto> ApprovalRequests { get; set; }

        public GetCustomApprovalRequestsOutput(RequestOutput result, List<GetCustomApprovalRequestForViewDto> approvalRequests)           
        {
            Result = result;
            ApprovalRequests = approvalRequests;
        }
    }
}