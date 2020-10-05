using QiProcureDemo.Products;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ProductPrices.Exporting;
using QiProcureDemo.ProductPrices.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.ProductPrices
{
	[AbpAuthorize(AppPermissions.Pages_Management_ProductPrices)]
    public class ProductPricesAppService : QiProcureDemoAppServiceBase, IProductPricesAppService
    {
		 private readonly IRepository<ProductPrice> _productPriceRepository;
		 private readonly IProductPricesExcelExporter _productPricesExcelExporter;
		 private readonly IRepository<Product,int> _lookup_productRepository;
		 

		  public ProductPricesAppService(IRepository<ProductPrice> productPriceRepository, IProductPricesExcelExporter productPricesExcelExporter , IRepository<Product, int> lookup_productRepository) 
		  {
			_productPriceRepository = productPriceRepository;
			_productPricesExcelExporter = productPricesExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductPriceForViewDto>> GetAll(GetAllProductPricesInput input)
         {
			
			var filteredProductPrices = _productPriceRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.MinvalidityFilter != null, e => e.validity >= input.MinvalidityFilter)
						.WhereIf(input.MaxvalidityFilter != null, e => e.validity <= input.MaxvalidityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredProductPrices = filteredProductPrices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productPrices = from o in pagedAndFilteredProductPrices
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductPriceForViewDto() {
							ProductPrice = new ProductPriceDto
							{
                                Price = o.Price,
                                validity = o.validity,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProductPrices.CountAsync();

            return new PagedResultDto<GetProductPriceForViewDto>(
                totalCount,
                await productPrices.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetProductPriceForViewDto>> GetByProductId(GetAllProductPricesInput input)
        {

            var filteredProductPrices = _productPriceRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .Where(e => e.ProductId == input.ProductIdFilter);

            var pagedAndFilteredProductPrices = filteredProductPrices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productPrices = from o in pagedAndFilteredProductPrices
                                join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new GetProductPriceForViewDto()
                                {
                                    ProductPrice = new ProductPriceDto
                                    {
                                        Price = o.Price,
                                        validity = o.validity,
                                        Id = o.Id
                                    },
                                    ProductName = s1 == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredProductPrices.CountAsync();

            return new PagedResultDto<GetProductPriceForViewDto>(
                totalCount,
                await productPrices.ToListAsync()
            );
        }

        public async Task<GetProductPriceForViewDto> GetProductPriceForView(int id)
         {
            var productPrice = await _productPriceRepository.GetAsync(id);

            var output = new GetProductPriceForViewDto { ProductPrice = ObjectMapper.Map<ProductPriceDto>(productPrice) };

		    if (output.ProductPrice.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductPrice.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_ProductPrices_Edit)]
		 public async Task<GetProductPriceForEditOutput> GetProductPriceForEdit(EntityDto input)
         {
            var productPrice = await _productPriceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductPriceForEditOutput {ProductPrice = ObjectMapper.Map<CreateOrEditProductPriceDto>(productPrice)};

		    if (output.ProductPrice.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductPrice.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductPriceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ProductPrices_Create)]
		 protected virtual async Task Create(CreateOrEditProductPriceDto input)
         {
            var productPrice = ObjectMapper.Map<ProductPrice>(input);

			
			if (AbpSession.TenantId != null)
			{
				productPrice.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productPriceRepository.InsertAsync(productPrice);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ProductPrices_Edit)]
		 protected virtual async Task Update(CreateOrEditProductPriceDto input)
         {
            var productPrice = await _productPriceRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, productPrice);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ProductPrices_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _productPriceRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductPricesToExcel(GetAllProductPricesForExcelInput input)
         {
			
			var filteredProductPrices = _productPriceRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.MinvalidityFilter != null, e => e.validity >= input.MinvalidityFilter)
						.WhereIf(input.MaxvalidityFilter != null, e => e.validity <= input.MaxvalidityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredProductPrices
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductPriceForViewDto() { 
							ProductPrice = new ProductPriceDto
							{
                                Price = o.Price,
                                validity = o.validity,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productPriceListDtos = await query.ToListAsync();

            return _productPricesExcelExporter.ExportToFile(productPriceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Management_ProductPrices)]
         public async Task<PagedResultDto<ProductPriceProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductPriceProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductPriceProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductPriceProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}