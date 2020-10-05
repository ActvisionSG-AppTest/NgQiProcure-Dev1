using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ProductCategories.Dtos;
using QiProcureDemo.Dto;
using static QiProcureDemo.Helpers.Product.ProductNameValue;
using System.Collections.Generic;

namespace QiProcureDemo.ProductCategories
{
    public interface IProductCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input);

        Task<GetProductCategoryForViewDto> GetProductCategoryForView(int id);

		Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProductCategoryDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<ProductCategoryProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProductCategoryCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);
		
    }
}