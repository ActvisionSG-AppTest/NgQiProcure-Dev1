
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Teams.Dtos
{
    public class CreateOrEditTeamDto : EntityDto<int?>
    {

		[Required]
		[StringLength(TeamConsts.MaxNameLength, MinimumLength = TeamConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(TeamConsts.MaxDescriptionLength, MinimumLength = TeamConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		[StringLength(TeamConsts.MaxRemarkLength, MinimumLength = TeamConsts.MinRemarkLength)]
		public string Remark { get; set; }
		
		
		 public int? SysStatusId { get; set; }
		 
		 		 public int? ReferenceTypeId { get; set; }
		 
		 
    }
}