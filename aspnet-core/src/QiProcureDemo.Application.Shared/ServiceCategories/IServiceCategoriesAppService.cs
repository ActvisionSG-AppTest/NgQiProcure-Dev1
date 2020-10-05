using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ServiceCategories.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ServiceCategories
{
    public interface IServiceCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetServiceCategoryForViewDto>> GetAll(GetAllServiceCategoriesInput input);

        Task<GetServiceCategoryForViewDto> GetServiceCategoryForView(int id);

		Task<GetServiceCategoryForEditOutput> GetServiceCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditServiceCategoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetServiceCategoriesToExcel(GetAllServiceCategoriesForExcelInput input);

		
		Task<PagedResultDto<ServiceCategoryServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ServiceCategoryCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);
		
    }
}