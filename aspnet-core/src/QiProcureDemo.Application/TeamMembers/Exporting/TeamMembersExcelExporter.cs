using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.TeamMembers.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.TeamMembers.Exporting
{
    public class TeamMembersExcelExporter : NpoiExcelExporterBase, ITeamMembersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TeamMembersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTeamMemberForViewDto> teamMembers)
        {
            return CreateExcelPackage(
                "TeamMembers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TeamMembers"));
                    

                    AddHeader(
                        sheet,
                        L("Remark"),
                        L("ReportingTeamMemberId"),
                        (L("Team")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("SysRef")) + L("TenantId"),
                        (L("SysStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, teamMembers,
                        _ => _.TeamMember.Remark,
                        _ => _.TeamMember.ReportingTeamMemberId,
                        _ => _.TeamName,
                        _ => _.UserName,
                        _ => _.SysRefTenantId,
                        _ => _.SysStatusName
                        );


                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
