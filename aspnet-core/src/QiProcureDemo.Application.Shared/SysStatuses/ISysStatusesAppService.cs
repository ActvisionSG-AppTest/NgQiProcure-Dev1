using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.SysStatuses.Dtos;
using QiProcureDemo.Dto;
using System.Collections.Generic;

namespace QiProcureDemo.SysStatuses
{
    public interface ISysStatusesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSysStatusForViewDto>> GetAll(GetAllSysStatusesInput input);

        Task<GetSysStatusForViewDto> GetSysStatusForView(int id);

		GetSysStatusForViewDto GetNextSysStatus(int? currentStatusCode, string refCode, string nextStatusName);

		Task<GetSysStatusForEditOutput> GetSysStatusForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSysStatusDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSysStatusesToExcel(GetAllSysStatusesForExcelInput input);
		
		Task<PagedResultDto<SysStatusesysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);
		
    }
}