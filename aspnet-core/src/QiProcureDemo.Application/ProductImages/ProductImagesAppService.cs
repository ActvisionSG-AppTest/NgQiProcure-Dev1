using QiProcureDemo.Products;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ProductImages.Exporting;
using QiProcureDemo.ProductImages.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Caching;
using QiProcureDemo.Storage;
using Abp.UI;
using System.Drawing;
using System.IO;

namespace QiProcureDemo.ProductImages
{
	[AbpAuthorize(AppPermissions.Pages_Management_ProductImages)]
    public class ProductImagesAppService : QiProcureDemoAppServiceBase, IProductImagesAppService
    {
        private const int MaxProductImagesBytes = 5242880; //5MB
        private readonly IRepository<ProductImage> _productImageRepository;
		private readonly IProductImagesExcelExporter _productImagesExcelExporter;
		private readonly IRepository<Product,int> _lookup_productRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public ProductImagesAppService(IRepository<ProductImage> productImageRepository, IProductImagesExcelExporter productImagesExcelExporter , IRepository<Product, int> lookup_productRepository,
            ICacheManager cacheManager,
            ITempFileCacheManager tempFileCacheManager) 
		  {
			_productImageRepository = productImageRepository;
			_productImagesExcelExporter = productImagesExcelExporter;
			_lookup_productRepository = lookup_productRepository;
            _cacheManager = cacheManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

		 public async Task<PagedResultDto<GetProductImageForViewDto>> GetAll(GetAllProductImagesInput input)
         {
			
			var filteredProductImages = _productImageRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter),  e => e.Url == input.UrlFilter)
						.WhereIf(input.IsMainFilter > -1,  e => (input.IsMainFilter == 1 && e.IsMain) || (input.IsMainFilter == 0 && !e.IsMain) )
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredProductImages = filteredProductImages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productImages = from o in pagedAndFilteredProductImages
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductImageForViewDto() {
							ProductImage = new ProductImageDto
							{
                                Description = o.Description,
                                Url = o.Url,
                                IsMain = o.IsMain,
                                IsApproved = o.IsApproved,
                                Id = o.Id,
                                ProductId = o.ProductId
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProductImages.CountAsync();

            return new PagedResultDto<GetProductImageForViewDto>(
                totalCount,
                await productImages.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetProductImageForViewDto>> GetProductImagesByProductId(int ProductId)
        {

            var filteredProductImages = _productImageRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(ProductId != 0, e => e.ProductId == ProductId && e.IsDeleted == false );

            var pagedAndFilteredProductImages = filteredProductImages
                .OrderBy("id asc");

            var productImages = from o in pagedAndFilteredProductImages
                                join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new GetProductImageForViewDto()
                                {
                                    ProductImage = new ProductImageDto
                                    {
                                        Description = o.Description,
                                        Url = o.Url,
                                        IsMain = o.IsMain,
                                        IsApproved = o.IsApproved,
                                        Id = o.Id,
                                        ProductId = o.ProductId,
                                        Bytes = o.Bytes,
                                        ImageBase64String = Convert.ToBase64String(o.Bytes)

                                    },
                                    ProductName = s1 == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredProductImages.CountAsync();

            return new PagedResultDto<GetProductImageForViewDto>(
                totalCount,
                await productImages.ToListAsync()
            );
        }
        public async Task<GetProductImageForViewDto> GetProductImageForView(int id)
         {
            var productImage = await _productImageRepository.GetAsync(id);

            var output = new GetProductImageForViewDto { ProductImage = ObjectMapper.Map<ProductImageDto>(productImage) };

		    if (output.ProductImage.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductImage.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_ProductImages_Edit)]
		 public async Task<GetProductImageForEditOutput> GetProductImageForEdit(EntityDto input)
         {
            var productImage = await _productImageRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductImageForEditOutput {ProductImage = ObjectMapper.Map<CreateOrEditProductImageDto>(productImage)};

		    if (output.ProductImage.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductImage.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductImageDto input)
         {
            if(input.Id == null){
				await Create(input);

			}
			else{
				await Update(input);
			}
         }
        public async Task UpdateProductImages(UpdateProductImagesInput inputProductImages)
        {

            byte[] byteArray;
            string _fileType;
            string[] _fileExtArr;
            string _fileExt = "jpeg";
            var imageBytes = _tempFileCacheManager.GetFile(inputProductImages.FileToken);
            if (inputProductImages.FileType != null)
            {
                _fileType = inputProductImages.FileType;
                _fileExtArr = _fileType.Split("/");
                if (_fileExtArr.Length > 1)
                    _fileExt = _fileExtArr[1];
            }
                

            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + inputProductImages.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (inputProductImages.Width == 0 || inputProductImages.Width > bmpImage.Width) ? bmpImage.Width : inputProductImages.Width;
                var height = (inputProductImages.Height == 0 || inputProductImages.Height > bmpImage.Height) ? bmpImage.Height : inputProductImages.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(inputProductImages.X, inputProductImages.Y, width, height), bmpImage.PixelFormat);

                using (var stream = new MemoryStream())
                {
                    bmCrop.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            if (byteArray.Length > MaxProductImagesBytes)
            {
                throw new UserFriendlyException(L("Image_Warn_SizeLimit", AppConsts.MaxImageBytesUserFriendlyValue));
            }

            var inputDto = new ProductImageDto
            {
                ProductId = inputProductImages.ProductId,
                Url = inputProductImages.FileToken + "." + _fileExt,
                Description = inputProductImages.Description
            };

            var productImageDto = ObjectMapper.Map<ProductImage>(inputDto);

            
            if (AbpSession.TenantId != null)
            {
                productImageDto.TenantId = (int?)AbpSession.TenantId;                
            }

            productImageDto.Bytes = byteArray;
            
            await _productImageRepository.InsertAsync(productImageDto);
        }
        [AbpAuthorize(AppPermissions.Pages_Management_ProductImages_Create)]
		 protected virtual async Task Create(CreateOrEditProductImageDto input)
         {
            var productImage = ObjectMapper.Map<ProductImage>(input);

			
			if (AbpSession.TenantId != null)
			{
				productImage.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productImageRepository.InsertAsync(productImage);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ProductImages_Edit)]
		 protected virtual async Task Update(CreateOrEditProductImageDto input)
         {
            var productImage = await _productImageRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, productImage);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ProductImages_Delete)]
        public async Task Delete(EntityDto input)
         {
            await _productImageRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductImagesToExcel(GetAllProductImagesForExcelInput input)
         {
			
			var filteredProductImages = _productImageRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter),  e => e.Url == input.UrlFilter)
						.WhereIf(input.IsMainFilter > -1,  e => (input.IsMainFilter == 1 && e.IsMain) || (input.IsMainFilter == 0 && !e.IsMain) )
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredProductImages
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductImageForViewDto() { 
							ProductImage = new ProductImageDto
							{
                                Description = o.Description,
                                Url = o.Url,
                                IsMain = o.IsMain,
                                IsApproved = o.IsApproved,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productImageListDtos = await query.ToListAsync();

            return _productImagesExcelExporter.ExportToFile(productImageListDtos);
         }

		[AbpAuthorize(AppPermissions.Pages_Management_ProductImages)]
         public async Task<PagedResultDto<ProductImageProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductImageProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductImageProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductImageProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}