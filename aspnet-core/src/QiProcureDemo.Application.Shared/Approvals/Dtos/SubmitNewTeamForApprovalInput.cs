using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace QiProcureDemo.Approvals.Dtos
{
    public class SubmitNewTeamForApprovalInput
    {
        [Required]       
        public int TeamId { get; set; }

        public int SysStatusId { get; set; }
    }
}