using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.Teams.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Teams.Exporting
{
    public class TeamsExcelExporter : NpoiExcelExporterBase, ITeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTeamForViewDto> teams)
        {
            return CreateExcelPackage(
                "Teams.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Teams"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("IsActive"),
                        L("Remark"),
                        (L("SysStatus")) + L("Name"),
                        (L("ReferenceType")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, teams,
                        _ => _.Team.Name,
                        _ => _.Team.Description,
                        _ => _.Team.IsActive,
                        _ => _.Team.Remark,
                        _ => _.SysStatusName,
                        _ => _.ReferenceTypeName
                        );


                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
