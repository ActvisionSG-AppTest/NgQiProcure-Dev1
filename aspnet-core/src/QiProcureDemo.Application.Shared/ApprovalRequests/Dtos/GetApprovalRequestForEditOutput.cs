using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ApprovalRequests.Dtos
{
    public class GetApprovalRequestForEditOutput
    {
		public CreateOrEditApprovalRequestDto ApprovalRequest { get; set; }

		public string SysRefTenantId { get; set;}

		public string SysStatusName { get; set;}

		public string UserName { get; set;}


    }
}