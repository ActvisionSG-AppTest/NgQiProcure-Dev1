using System.Collections.Generic;
using QiProcureDemo.Emails.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Emails.Exporting
{
    public interface IEmailsExcelExporter
    {
        FileDto ExportToFile(List<GetEmailForViewDto> emails);
    }
}