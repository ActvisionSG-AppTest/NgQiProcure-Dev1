using System.Collections.Generic;
using QiProcureDemo.Accounts.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Accounts.Exporting
{
    public interface IAccountsExcelExporter
    {
        FileDto ExportToFile(List<GetAccountForViewDto> accounts);
    }
}