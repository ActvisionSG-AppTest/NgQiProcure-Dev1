using System.Collections.Generic;
using Abp;
using QiProcureDemo.Chat.Dto;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
