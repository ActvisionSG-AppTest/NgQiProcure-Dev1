
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Projects.Dtos
{
    public class ProjectDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public bool IsApprove { get; set; }

		public bool IsActive { get; set; }

		public bool Publish { get; set; }

		public string Remark { get; set; }


		 public int? AccountId { get; set; }

		 		 public int? TeamId { get; set; }

		 		 public int? SysStatusId { get; set; }

		 
    }
}