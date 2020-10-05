using QiProcureDemo.Products;
using QiProcureDemo.Categories;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ProductCategories.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp;

namespace QiProcureDemo.ProductCategories
{
	[AbpAuthorize(AppPermissions.Pages_Management_ProductCategories)]
    public class ProductCategoriesAppService : QiProcureDemoAppServiceBase, IProductCategoriesAppService
    {
		 private readonly IRepository<ProductCategory> _productCategoryRepository;
		 private readonly IRepository<Product,int> _lookup_productRepository;
		 private readonly IRepository<Category,int> _lookup_categoryRepository;
		 

		  public ProductCategoriesAppService(IRepository<ProductCategory> productCategoryRepository , IRepository<Product, int> lookup_productRepository, IRepository<Category, int> lookup_categoryRepository) 
		  {
			_productCategoryRepository = productCategoryRepository;
			_lookup_productRepository = lookup_productRepository;
		    _lookup_categoryRepository = lookup_categoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input)
         {
			
			var filteredProductCategories = _productCategoryRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.CategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

			var pagedAndFilteredProductCategories = filteredProductCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productCategories = from o in pagedAndFilteredProductCategories
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetProductCategoryForViewDto() {
							ProductCategory = new ProductCategoryDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	CategoryName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredProductCategories.CountAsync();

            return new PagedResultDto<GetProductCategoryForViewDto>(
                totalCount,
                await productCategories.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetProductCategoryForViewDto>> GetProductCategory(int ProductId, int CategoryId)
        {

            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(ProductId != 0, e => e.ProductId == ProductId && e.CategoryId == CategoryId);


            var productCategories = from o in filteredProductCategories
                                    join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new GetProductCategoryForViewDto()
                                    {
                                        ProductCategory = new ProductCategoryDto
                                        {
                                            Id = o.Id
                                        },
                                        ProductName = s1 == null ? "" : s1.Name.ToString(),
                                        CategoryName = s2 == null ? "" : s2.Name.ToString()
                                        
                                    };

            var totalCount = await filteredProductCategories.CountAsync();

            return new PagedResultDto<GetProductCategoryForViewDto>(
                totalCount,
                await productCategories.ToListAsync()
            );
        }

        public List<NameValue<string>> GetProductCategoriesNameValue(int ProductId)
        {

            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(ProductId != 0, e => e.ProductId == ProductId);

            var pagedAndFilteredProductCategories = filteredProductCategories
                  .OrderBy("Id asc");

            var productCategories = (from o in filteredProductCategories
                                    join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()
                                                     
                                    select new GetProductCategoryForViewDto()
                                    {
                                        ProductCategory = new ProductCategoryDto
                                        {
                                            CategoryId = o.CategoryId
                                        },
                                        
                                        CategoryName = s2 == null ? "" : s2.Name.ToString()
                                    }).Distinct();

            var productCategoryList = new List<NameValue<string>> { };

            foreach (var productCategory in productCategories)
            {
                productCategoryList.Add(new NameValue { Name = productCategory.CategoryName, Value = productCategory.ProductCategory.CategoryId.ToString() });
            }

            return productCategoryList;
        }


        [AbpAuthorize(AppPermissions.Pages_Management_ProductCategories_Delete)]
        public virtual async Task sendAndGetSelectedCategories(List<NameValueProduct> selectedCategories)
        {

            int i = 0;

            foreach (var category in selectedCategories)
            {
                var filteredProductCategories = _productCategoryRepository.GetAll()
                                           .Include(e => e.ProductFk)
                                           .Include(e => e.CategoryFk)
                                           .Where(e => (e.ProductId == Convert.ToInt32(category.ProductId)) && e.CategoryId == Convert.ToInt32(category.Value));

                var totalCount = await filteredProductCategories.CountAsync();
                if (totalCount == 0)
                {


                    var productCategory = new ProductCategory
                    {
                        ProductId = Convert.ToInt32(category.ProductId),
                        CategoryId = (Convert.ToInt32(category.Value))
                    };
                    if (AbpSession.TenantId != null)
                    {
                        productCategory.TenantId = (int?)AbpSession.TenantId;
                    }
                    await _productCategoryRepository.InsertAsync(productCategory);
                }

            }
        }

        public class NameValueProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string ProductId { get; set; }

        }
        public async Task<GetProductCategoryForViewDto> GetProductCategoryForView(int id)
         {
            var productCategory = await _productCategoryRepository.GetAsync(id);

            var output = new GetProductCategoryForViewDto { ProductCategory = ObjectMapper.Map<ProductCategoryDto>(productCategory) };

		    if (output.ProductCategory.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductCategory.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.ProductCategory.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.ProductCategory.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_ProductCategories_Edit)]
		 public async Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto input)
         {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductCategoryForEditOutput {ProductCategory = ObjectMapper.Map<CreateOrEditProductCategoryDto>(productCategory)};

		    if (output.ProductCategory.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductCategory.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.ProductCategory.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.ProductCategory.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductCategoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }
       

         [AbpAuthorize(AppPermissions.Pages_Management_ProductCategories_Create)]		 
        protected virtual async Task Create(CreateOrEditProductCategoryDto input)
         {
            var productCategory = ObjectMapper.Map<ProductCategory>(input);

			
			if (AbpSession.TenantId != null)
			{
				productCategory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productCategoryRepository.InsertAsync(productCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ProductCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditProductCategoryDto input)
         {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, productCategory);
         }


        [AbpAuthorize(AppPermissions.Pages_Management_ProductCategories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _productCategoryRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_Management_ProductCategories)]
         public async Task<PagedResultDto<ProductCategoryProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductCategoryProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductCategoryProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductCategoryProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Management_ProductCategories)]
         public async Task<PagedResultDto<ProductCategoryCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_categoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductCategoryCategoryLookupTableDto>();
			foreach(var category in categoryList){
				lookupTableDtoList.Add(new ProductCategoryCategoryLookupTableDto
				{
					Id = category.Id,
					DisplayName = category.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductCategoryCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}