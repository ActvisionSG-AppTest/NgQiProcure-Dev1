using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ProjectInstructions.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ProjectInstructions.Exporting
{
    public class ProjectInstructionsExcelExporter : NpoiExcelExporterBase, IProjectInstructionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectInstructionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectInstructionForViewDto> projectInstructions)
        {
            return CreateExcelPackage(
                "ProjectInstructions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ProjectInstructions"));
                    

                    AddHeader(
                        sheet,
                        L("InstructionNo"),
                        L("Instructions"),
                        L("Remarks"),
                        L("IsActive"),
                        (L("Project")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, projectInstructions,
                        _ => _.ProjectInstruction.InstructionNo,
                        _ => _.ProjectInstruction.Instructions,
                        _ => _.ProjectInstruction.Remarks,
                        _ => _.ProjectInstruction.IsActive,
                        _ => _.ProjectName
                        );

                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


                });
        }
    }
}
