using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Projects.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Projects.Exporting
{
    public class ProjectsExcelExporter : NpoiExcelExporterBase, IProjectsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectForViewDto> projects)
        {
            return CreateExcelPackage(
                "Projects.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Projects"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("StartDate"),
                        L("EndDate"),
                        L("IsApprove"),
                        L("IsActive"),
                        L("Publish"),
                        L("Remark"),
                        (L("Account")) + L("Name"),
                        (L("Team")) + L("Name"),
                        (L("SysStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, projects,
                        _ => _.Project.Name,
                        _ => _.Project.Description,
                        _ => _timeZoneConverter.Convert(_.Project.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Project.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Project.IsApprove,
                        _ => _.Project.IsActive,
                        _ => _.Project.Publish,
                        _ => _.Project.Remark,
                        _ => _.AccountName,
                        _ => _.TeamName,
                        _ => _.SysStatusName
                        );

                    for (var i = 1; i <= projects.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }

                    for (var i = 0; i < 11; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
