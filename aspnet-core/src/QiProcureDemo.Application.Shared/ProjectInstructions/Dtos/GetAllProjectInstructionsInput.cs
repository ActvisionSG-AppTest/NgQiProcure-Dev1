using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ProjectInstructions.Dtos
{
    public class GetAllProjectInstructionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxInstructionNoFilter { get; set; }
		public int? MinInstructionNoFilter { get; set; }

		public string InstructionsFilter { get; set; }

		public string RemarksFilter { get; set; }

		public int IsActiveFilter { get; set; }


		 public string ProjectNameFilter { get; set; }

		 
    }
}