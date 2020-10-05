using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ProductPrices.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ProductPrices
{
    public interface IProductPricesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductPriceForViewDto>> GetAll(GetAllProductPricesInput input);

        Task<GetProductPriceForViewDto> GetProductPriceForView(int id);

		Task<GetProductPriceForEditOutput> GetProductPriceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProductPriceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProductPricesToExcel(GetAllProductPricesForExcelInput input);

		
		Task<PagedResultDto<ProductPriceProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}