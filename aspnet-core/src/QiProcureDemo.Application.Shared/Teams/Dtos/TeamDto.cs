
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Teams.Dtos
{
    public class TeamDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public bool IsActive { get; set; }

		public string Remark { get; set; }


		 public int? SysStatusId { get; set; }

		 		 public int? ReferenceTypeId { get; set; }

		 
    }
}