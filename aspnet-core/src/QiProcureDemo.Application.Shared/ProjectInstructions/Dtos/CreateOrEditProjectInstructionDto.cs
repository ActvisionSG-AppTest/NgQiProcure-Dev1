
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProjectInstructions.Dtos
{
    public class CreateOrEditProjectInstructionDto : EntityDto<int?>
    {

		[Range(ProjectInstructionConsts.MinInstructionNoValue, ProjectInstructionConsts.MaxInstructionNoValue)]
		public int InstructionNo { get; set; }
		
		
		[StringLength(ProjectInstructionConsts.MaxInstructionsLength, MinimumLength = ProjectInstructionConsts.MinInstructionsLength)]
		public string Instructions { get; set; }
		
		
		[StringLength(ProjectInstructionConsts.MaxRemarksLength, MinimumLength = ProjectInstructionConsts.MinRemarksLength)]
		public string Remarks { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		 public int? ProjectId { get; set; }
		 
		 
    }
}