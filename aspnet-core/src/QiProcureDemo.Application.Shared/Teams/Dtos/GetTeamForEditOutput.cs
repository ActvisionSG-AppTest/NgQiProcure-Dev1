using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Teams.Dtos
{
    public class GetTeamForEditOutput
    {
		public CreateOrEditTeamDto Team { get; set; }

		public string SysStatusName { get; set;}

		public string ReferenceTypeName { get; set;}


    }
}