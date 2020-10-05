using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ParamSettings.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.ParamSettings
{
    public interface IParamSettingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetParamSettingForViewDto>> GetAll(GetAllParamSettingsInput input);

        Task<GetParamSettingForViewDto> GetParamSettingForView(int id);

		Task<GetParamSettingForEditOutput> GetParamSettingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditParamSettingDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetParamSettingsToExcel(GetAllParamSettingsForExcelInput input);

		
    }
}