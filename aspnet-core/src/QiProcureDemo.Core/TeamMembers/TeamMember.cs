using QiProcureDemo.Teams;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.SysRefs;
using QiProcureDemo.SysStatuses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace QiProcureDemo.TeamMembers
{
	[Table("QP_TeamMembers")]
    public class TeamMember : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(TeamMemberConsts.MaxRemarkLength, MinimumLength = TeamMemberConsts.MinRemarkLength)]
		public virtual string Remark { get; set; }
		
		public virtual int ReportingTeamMemberId { get; set; }
		

		public virtual int? TeamId { get; set; }
		
        [ForeignKey("TeamId")]
		public Team TeamFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? SysRefId { get; set; }
		
        [ForeignKey("SysRefId")]
		public SysRef SysRefFk { get; set; }
		
		public virtual int? SysStatusId { get; set; }
		
        [ForeignKey("SysStatusId")]
		public SysStatus SysStatusFk { get; set; }
		
    }
}