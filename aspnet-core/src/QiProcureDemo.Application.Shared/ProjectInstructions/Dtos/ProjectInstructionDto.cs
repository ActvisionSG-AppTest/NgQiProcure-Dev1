
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProjectInstructions.Dtos
{
    public class ProjectInstructionDto : EntityDto
    {
		public int InstructionNo { get; set; }

		public string Instructions { get; set; }

		public string Remarks { get; set; }

		public bool IsActive { get; set; }


		 public int? ProjectId { get; set; }

		 
    }
}