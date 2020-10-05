using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.TeamMembers.Dtos
{
    public class GetTeamMemberForEditOutput
    {
		public CreateOrEditTeamMemberDto TeamMember { get; set; }

		public string TeamName { get; set;}

		public string UserName { get; set;}

		public string SysRefTenantId { get; set;}

		public string SysStatusName { get; set;}


    }
}