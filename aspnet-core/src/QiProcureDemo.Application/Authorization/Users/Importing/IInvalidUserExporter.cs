using System.Collections.Generic;
using QiProcureDemo.Authorization.Users.Importing.Dto;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
