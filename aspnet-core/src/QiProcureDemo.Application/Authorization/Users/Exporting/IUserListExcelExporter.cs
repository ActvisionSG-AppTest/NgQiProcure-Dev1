using System.Collections.Generic;
using QiProcureDemo.Authorization.Users.Dto;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}