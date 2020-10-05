using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Accounts.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Accounts.Exporting
{
    public class AccountsExcelExporter : NpoiExcelExporterBase, IAccountsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AccountsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAccountForViewDto> accounts)
        {
            return CreateExcelPackage(
                "Accounts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Accounts"));
                    
                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("IsPersonal"),
                        L("IsActive"),
                        L("Remark"),
                        L("Code"),
                        L("Email"),
                        L("UserName"),
                        L("Password"),
                        (L("Team")) + L("Name"),
                        (L("SysStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, accounts,
                        _ => _.Account.Name,
                        _ => _.Account.Description,
                        _ => _.Account.IsPersonal,
                        _ => _.Account.IsActive,
                        _ => _.Account.Remark,
                        _ => _.Account.Code,
                        _ => _.Account.Email,
                        _ => _.Account.UserName,
                        _ => _.Account.Password,
                        _ => _.TeamName,
                        _ => _.SysStatusName
                        );


                    for (var i = 0; i < 11; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
