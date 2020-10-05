
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Projects.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ProjectConsts.MaxNameLength, MinimumLength = ProjectConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(ProjectConsts.MaxDescriptionLength, MinimumLength = ProjectConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		public DateTime? StartDate { get; set; }
		
		
		public DateTime? EndDate { get; set; }
		
		
		public bool IsApprove { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		public bool Publish { get; set; }
		
		
		[StringLength(ProjectConsts.MaxRemarkLength, MinimumLength = ProjectConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		 public int? AccountId { get; set; }
		 
		 		 public int? TeamId { get; set; }
		 
		 		 public int? SysStatusId { get; set; }
		 
		 
    }
}