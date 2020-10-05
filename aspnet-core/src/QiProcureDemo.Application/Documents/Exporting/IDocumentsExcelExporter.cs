using System.Collections.Generic;
using QiProcureDemo.Documents.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Documents.Exporting
{
    public interface IDocumentsExcelExporter
    {
        FileDto ExportToFile(List<GetDocumentForViewDto> documents);
    }
}