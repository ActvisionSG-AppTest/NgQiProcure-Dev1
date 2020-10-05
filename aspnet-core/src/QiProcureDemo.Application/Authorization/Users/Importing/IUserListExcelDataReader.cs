using System.Collections.Generic;
using QiProcureDemo.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace QiProcureDemo.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
