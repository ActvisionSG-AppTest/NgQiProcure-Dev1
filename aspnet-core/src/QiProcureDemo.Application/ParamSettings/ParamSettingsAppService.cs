

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ParamSettings.Exporting;
using QiProcureDemo.ParamSettings.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.ParamSettings
{
	[AbpAuthorize(AppPermissions.Pages_Administration_ParamSettings)]
    public class ParamSettingsAppService : QiProcureDemoAppServiceBase, IParamSettingsAppService
    {
		 private readonly IRepository<ParamSetting> _paramSettingRepository;
		 private readonly IParamSettingsExcelExporter _paramSettingsExcelExporter;
		 

		  public ParamSettingsAppService(IRepository<ParamSetting> paramSettingRepository, IParamSettingsExcelExporter paramSettingsExcelExporter ) 
		  {
			_paramSettingRepository = paramSettingRepository;
			_paramSettingsExcelExporter = paramSettingsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetParamSettingForViewDto>> GetAll(GetAllParamSettingsInput input)
         {
			
			var filteredParamSettings = _paramSettingRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Value.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var pagedAndFilteredParamSettings = filteredParamSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var paramSettings = from o in pagedAndFilteredParamSettings
                         select new GetParamSettingForViewDto() {
							ParamSetting = new ParamSettingDto
							{
                                Name = o.Name,
                                Value = o.Value,
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredParamSettings.CountAsync();

            return new PagedResultDto<GetParamSettingForViewDto>(
                totalCount,
                await paramSettings.ToListAsync()
            );
         }
		

		public async Task<GetParamSettingForViewDto> GetParamSettingForView(int id)
         {
            var paramSetting = await _paramSettingRepository.GetAsync(id);

            var output = new GetParamSettingForViewDto { ParamSetting = ObjectMapper.Map<ParamSettingDto>(paramSetting) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ParamSettings_Edit)]
		 public async Task<GetParamSettingForEditOutput> GetParamSettingForEdit(EntityDto input)
         {
            var paramSetting = await _paramSettingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetParamSettingForEditOutput {ParamSetting = ObjectMapper.Map<CreateOrEditParamSettingDto>(paramSetting)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditParamSettingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ParamSettings_Create)]
		 protected virtual async Task Create(CreateOrEditParamSettingDto input)
         {
            var paramSetting = ObjectMapper.Map<ParamSetting>(input);

			
			if (AbpSession.TenantId != null)
			{
				paramSetting.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _paramSettingRepository.InsertAsync(paramSetting);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ParamSettings_Edit)]
		 protected virtual async Task Update(CreateOrEditParamSettingDto input)
         {
            var paramSetting = await _paramSettingRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, paramSetting);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ParamSettings_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _paramSettingRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetParamSettingsToExcel(GetAllParamSettingsForExcelInput input)
         {
			
			var filteredParamSettings = _paramSettingRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Value.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var query = (from o in filteredParamSettings
                         select new GetParamSettingForViewDto() { 
							ParamSetting = new ParamSettingDto
							{
                                Name = o.Name,
                                Value = o.Value,
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var paramSettingListDtos = await query.ToListAsync();

            return _paramSettingsExcelExporter.ExportToFile(paramSettingListDtos);
         }


    }
}