using System.Collections.Generic;
using QiProcureDemo.ParamSettings.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ParamSettings.Exporting
{
    public interface IParamSettingsExcelExporter
    {
        FileDto ExportToFile(List<GetParamSettingForViewDto> paramSettings);
    }
}