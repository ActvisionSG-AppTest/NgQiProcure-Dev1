using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Projects.Dtos
{
    public class GetProjectForEditOutput
    {
		public CreateOrEditProjectDto Project { get; set; }

		public string AccountName { get; set;}

		public string TeamName { get; set;}

		public string SysStatusName { get; set;}


    }
}