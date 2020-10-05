using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Services.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.SysRefs.Dtos.Custom;

namespace QiProcureDemo.Services
{
    public interface IServicesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetServiceForViewDto>> GetAll(GetAllServicesInput input);

        Task<GetServiceForViewDto> GetServiceForView(int id);

		Task<GetServiceForEditOutput> GetServiceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditServiceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetServicesToExcel(GetAllServicesForExcelInput input);

		
		Task<PagedResultDto<ServiceCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ServiceSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<ServiceSysRefLookupTableDto>> GetSysRefByRefType(GetAllSysRefsInput input);


	}
}