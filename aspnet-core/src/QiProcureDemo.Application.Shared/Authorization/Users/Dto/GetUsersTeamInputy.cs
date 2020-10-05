using System.Collections.Generic;
using Abp.Runtime.Validation;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Authorization.Users.Dto
{
    public class GetUsersTeamInput : PagedAndSortedInputDto, IShouldNormalize, IGetUsersInput
    {
        public string Filter { get; set; }

        public List<string> Permissions { get; set; }

        public int? Role { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }

            Filter = Filter?.Trim();
        }

        public int TeamId { get; set; }

        public bool isSelected { get; set; }

        public int? ReportingTeamMemberIdFilter { get; set; }
    }
}