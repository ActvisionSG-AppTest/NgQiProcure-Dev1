using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ServicePrices.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ServicePrices
{
    public interface IServicePricesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetServicePriceForViewDto>> GetAll(GetAllServicePricesInput input);

        Task<GetServicePriceForViewDto> GetServicePriceForView(int id);

		Task<GetServicePriceForEditOutput> GetServicePriceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditServicePriceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetServicePricesToExcel(GetAllServicePricesForExcelInput input);

		
		Task<PagedResultDto<ServicePriceServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input);
		
    }
}