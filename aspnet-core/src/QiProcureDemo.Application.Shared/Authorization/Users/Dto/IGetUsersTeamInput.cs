using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace QiProcureDemo.Authorization.Users.Dto
{
    public interface IGetUsersTeamInput : ISortedResultRequest
    {
        string Filter { get; set; }

        List<string> Permissions { get; set; }

        int? Role { get; set; }

        bool OnlyLockedUsers { get; set; }

        int TeamId { get; set; }
    }
}