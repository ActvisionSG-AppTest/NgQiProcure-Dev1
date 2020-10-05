using QiProcureDemo.Categories;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Products.Exporting;
using QiProcureDemo.Products.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.Products
{
	[AbpAuthorize(AppPermissions.Pages_Management_Product_Products)]
    public class ProductsAppService : QiProcureDemoAppServiceBase, IProductsAppService
    {
		 private readonly IRepository<Product> _productRepository;
		 private readonly IProductsExcelExporter _productsExcelExporter;
		 private readonly IRepository<Category,int> _lookup_categoryRepository;
		 

		  public ProductsAppService(IRepository<Product> productRepository, IProductsExcelExporter productsExcelExporter , IRepository<Category, int> lookup_categoryRepository) 
		  {
			_productRepository = productRepository;
			_productsExcelExporter = productsExcelExporter;
			_lookup_categoryRepository = lookup_categoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
         {
			
			var filteredProducts = _productRepository.GetAll()
						.Include( e => e.CategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Uom.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinStockFilter != null, e => e.Stock >= input.MinStockFilter)
						.WhereIf(input.MaxStockFilter != null, e => e.Stock <= input.MaxStockFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UomFilter),  e => e.Uom == input.UomFilter)
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

			var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var products = from o in pagedAndFilteredProducts
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductForViewDto() {
							Product = new ProductDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Stock = o.Stock,
                                Uom = o.Uom,
                                IsApproved = o.IsApproved,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	CategoryName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProducts.CountAsync();

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                await products.ToListAsync()
            );
         }
		 
		 public async Task<GetProductForViewDto> GetProductForView(int id)
         {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

		    if (output.Product.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Product.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_Product_Products_Edit)]
		 public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto input)
         {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductForEditOutput {Product = ObjectMapper.Map<CreateOrEditProductDto>(product)};

		    if (output.Product.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Product.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_Product_Products_Create)]
		 protected virtual async Task Create(CreateOrEditProductDto input)
         {
            var product = ObjectMapper.Map<Product>(input);

			
			if (AbpSession.TenantId != null)
			{
				product.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productRepository.InsertAsync(product);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_Product_Products_Edit)]
		 protected virtual async Task Update(CreateOrEditProductDto input)
         {
            var product = await _productRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, product);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_Product_Products_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _productRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
         {
			
			var filteredProducts = _productRepository.GetAll()
						.Include( e => e.CategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Uom.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinStockFilter != null, e => e.Stock >= input.MinStockFilter)
						.WhereIf(input.MaxStockFilter != null, e => e.Stock <= input.MaxStockFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UomFilter),  e => e.Uom == input.UomFilter)
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

			var query = (from o in filteredProducts
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductForViewDto() { 
							Product = new ProductDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Stock = o.Stock,
                                Uom = o.Uom,
                                IsApproved = o.IsApproved,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	CategoryName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Management_Product_Products)]
         public async Task<PagedResultDto<ProductCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_categoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductCategoryLookupTableDto>();
			foreach(var category in categoryList){
				lookupTableDtoList.Add(new ProductCategoryLookupTableDto
				{
					Id = category.Id,
					DisplayName = category.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}