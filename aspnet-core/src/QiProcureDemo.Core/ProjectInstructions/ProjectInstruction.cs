using QiProcureDemo.Projects;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.ProjectInstructions
{
	[Table("QP_ProjectInstructions")]
    public class ProjectInstruction : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(ProjectInstructionConsts.MinInstructionNoValue, ProjectInstructionConsts.MaxInstructionNoValue)]
		public virtual int InstructionNo { get; set; }
		
		[StringLength(ProjectInstructionConsts.MaxInstructionsLength, MinimumLength = ProjectInstructionConsts.MinInstructionsLength)]
		public virtual string Instructions { get; set; }
		
		[StringLength(ProjectInstructionConsts.MaxRemarksLength, MinimumLength = ProjectInstructionConsts.MinRemarksLength)]
		public virtual string Remarks { get; set; }
		
		public virtual bool IsActive { get; set; }
		

		public virtual int? ProjectId { get; set; }
		
        [ForeignKey("ProjectId")]
		public Project ProjectFk { get; set; }
		
    }
}