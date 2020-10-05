using System.Collections.Generic;
using QiProcureDemo.ReferenceTypes.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ReferenceTypes.Exporting
{
    public interface IReferenceTypesExcelExporter
    {
        FileDto ExportToFile(List<GetReferenceTypeForViewDto> referenceTypes);
    }
}