using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace QiProcureDemo.Authorization.Users.Dto
{
    public class UserProfileListDto : EntityDto<long>, IPassivable, IHasCreationTime
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public Guid ProfilePictureId { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public List<UserListRoleDto> Roles { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
        public string ProfilePicture { get; set; }

        public int TeamMemberId { get; set; }

        public int? SelectedRole { get; set; }

        public int? ReportingTo { get; set; }

        public int RoleOrderNumber { get; set; }

        public string SelectedTeamRoleName { get; set; }
    }
}