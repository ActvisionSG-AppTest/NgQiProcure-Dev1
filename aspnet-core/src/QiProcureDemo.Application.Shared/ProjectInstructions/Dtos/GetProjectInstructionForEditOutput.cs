using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProjectInstructions.Dtos
{
    public class GetProjectInstructionForEditOutput
    {
		public CreateOrEditProjectInstructionDto ProjectInstruction { get; set; }

		public string ProjectName { get; set;}


    }
}