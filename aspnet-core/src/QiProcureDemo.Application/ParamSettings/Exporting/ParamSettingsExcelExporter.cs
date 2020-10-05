using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ParamSettings.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ParamSettings.Exporting
{
    public class ParamSettingsExcelExporter : NpoiExcelExporterBase, IParamSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ParamSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetParamSettingForViewDto> paramSettings)
        {
            return CreateExcelPackage(
                "ParamSettings.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ParamSettings"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Value"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, paramSettings,
                        _ => _.ParamSetting.Name,
                        _ => _.ParamSetting.Value,
                        _ => _.ParamSetting.Description
                        );

                    for (var i = 0; i < 3; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
