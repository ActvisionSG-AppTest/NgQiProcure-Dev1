using QiProcureDemo.SysRefs;
using QiProcureDemo.Products;
using QiProcureDemo.Services;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Documents.Exporting;
using QiProcureDemo.Documents.Dtos;
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

namespace QiProcureDemo.Documents
{
	[AbpAuthorize(AppPermissions.Pages_Documents)]
    public class DocumentsAppService : QiProcureDemoAppServiceBase, IDocumentsAppService
    {
         private const int MaxProductImagesBytes = 5242880; //5MB
         private readonly IRepository<Document> _documentRepository;
		 private readonly IDocumentsExcelExporter _documentsExcelExporter;
		 private readonly IRepository<SysRef,int> _lookup_sysRefRepository;
		 private readonly IRepository<Product,int> _lookup_productRepository;
		 private readonly IRepository<Service,int> _lookup_serviceRepository;
         private readonly ICacheManager _cacheManager;
         private readonly ITempFileCacheManager _tempFileCacheManager;


        public DocumentsAppService(IRepository<Document> documentRepository, IDocumentsExcelExporter documentsExcelExporter , IRepository<SysRef, int> lookup_sysRefRepository, IRepository<Product, int> lookup_productRepository, IRepository<Service, int> lookup_serviceRepository,
            ICacheManager cacheManager,
            ITempFileCacheManager tempFileCacheManager) 
		  {
			_documentRepository = documentRepository;
			_documentsExcelExporter = documentsExcelExporter;
			_lookup_sysRefRepository = lookup_sysRefRepository;
		    _lookup_productRepository = lookup_productRepository;
		    _lookup_serviceRepository = lookup_serviceRepository;
            _cacheManager = cacheManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

		 public async Task<PagedResultDto<GetDocumentForViewDto>> GetAll(GetAllDocumentsInput input)
         {
			
			var filteredDocuments = _documentRepository.GetAll()
						.Include( e => e.SysRefFk)
						.Include( e => e.ProductFk)
						.Include( e => e.ServiceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Url.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter),  e => e.Url == input.UrlFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter);

			var pagedAndFilteredDocuments = filteredDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var documents = from o in pagedAndFilteredDocuments
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetDocumentForViewDto() {
							Document = new DocumentDto
							{
                                Url = o.Url,
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                         	ProductName = s2 == null ? "" : s2.Name.ToString(),
                         	ServiceName = s3 == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredDocuments.CountAsync();

            return new PagedResultDto<GetDocumentForViewDto>(
                totalCount,
                await documents.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetDocumentForViewDto>> GetDocuments(GetAllDocumentsInput input)
        {
            var filteredDocuments = _documentRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.ServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Url.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url == input.UrlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter)
                        .WhereIf(input.ProductIdFilter != 0, e => e.ProductFk != null && e.ProductId == input.ProductIdFilter)
                        .WhereIf(input.ServiceIdFilter != 0, e => e.ServiceFk != null && e.ServiceId == input.ServiceIdFilter);

            var pagedAndFilteredDocuments = filteredDocuments
                .OrderBy(input.Sorting ?? "Name asc")
                .PageBy(input);

            var documents = from o in pagedAndFilteredDocuments
                            join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            join o3 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o3.Id into j3
                            from s3 in j3.DefaultIfEmpty()

                            select new GetDocumentForViewDto()
                            {
                                Document = new DocumentDto
                                {
                                    Url = o.Url,
                                    Name = o.Name,
                                    Description = o.Description,
                                    Id = o.Id,
                                    ImageBase64String = Convert.ToBase64String(o.Bytes)
                                },
                                SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                ProductName = s2 == null ? "" : s2.Name.ToString(),
                                ServiceName = s3 == null ? "" : s3.Name.ToString()
                            };

            var totalCount = await filteredDocuments.CountAsync();

            return new PagedResultDto<GetDocumentForViewDto>(
                totalCount,
                await documents.ToListAsync()
            );
        }

        public async Task<GetDocumentForViewDto> GetDocumentForView(int id)
         {
            var document = await _documentRepository.GetAsync(id);

            var output = new GetDocumentForViewDto { Document = ObjectMapper.Map<DocumentDto>(document) };

		    if (output.Document.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Document.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.Document.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.Document.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.Document.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.Document.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Documents_Edit)]
		 public async Task<GetDocumentForEditOutput> GetDocumentForEdit(EntityDto input)
         {
            var document = await _documentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDocumentForEditOutput {Document = ObjectMapper.Map<CreateOrEditDocumentDto>(document)};

		    if (output.Document.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Document.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.Document.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.Document.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.Document.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.Document.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }
			
            return output;
         }

        public async Task UpdateDocuments(UpdateDocumentsInput inputDocuments)
        {

            byte[] byteArray;
            string _fileExt = inputDocuments.FileExt;
            var docBytes = _tempFileCacheManager.GetFile(inputDocuments.FileToken);
           /* if (inputDocuments.FileType != null)
            {
                _fileType = inputProductImages.FileType;
                _fileExtArr = _fileType.Split("/");
                if (_fileExtArr.Length > 1)
                    _fileExt = _fileExtArr[1];
            }*/


            if (docBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + inputDocuments.FileToken);
            }
            using (var docStream= new MemoryStream(docBytes))
            {
                byteArray = docStream.ToArray();
            }

            if (byteArray.Length > MaxProductImagesBytes)
            {
                throw new UserFriendlyException(L("Image_Warn_SizeLimit", AppConsts.MaxImageBytesUserFriendlyValue));
            }
            var inputDto = new DocumentDto { };
            if (inputDocuments.ProductId != 0)
            {
                inputDto = new DocumentDto
                {
                    ProductId = inputDocuments.ProductId,
                    Url = inputDocuments.FileToken + "." + _fileExt,
                    Description = inputDocuments.Description,
                    Name = inputDocuments.Name
                };
            } else
            {
                inputDto = new DocumentDto
                {
                    ServiceId = inputDocuments.ServiceId,
                    Url = inputDocuments.FileToken + "." + _fileExt,
                    Description = inputDocuments.Description,
                    Name = inputDocuments.Name
                };
            }
           

            var documentDto = ObjectMapper.Map<Document>(inputDto);


            if (AbpSession.TenantId != null)
            {
                documentDto.TenantId = (int?)AbpSession.TenantId;
            }

            documentDto.Bytes = byteArray;

            await _documentRepository.InsertAsync(documentDto);
        }
        public async Task CreateOrEdit(CreateOrEditDocumentDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Documents_Create)]
		 protected virtual async Task Create(CreateOrEditDocumentDto input)
         {
            var document = ObjectMapper.Map<Document>(input);

			
			if (AbpSession.TenantId != null)
			{
				document.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _documentRepository.InsertAsync(document);
         }

		 [AbpAuthorize(AppPermissions.Pages_Documents_Edit)]
		 protected virtual async Task Update(CreateOrEditDocumentDto input)
         {
            var document = await _documentRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, document);
         }

		 [AbpAuthorize(AppPermissions.Pages_Documents_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _documentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDocumentsToExcel(GetAllDocumentsForExcelInput input)
         {
			
			var filteredDocuments = _documentRepository.GetAll()
						.Include( e => e.SysRefFk)
						.Include( e => e.ProductFk)
						.Include( e => e.ServiceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Url.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter),  e => e.Url == input.UrlFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter);

			var query = (from o in filteredDocuments
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetDocumentForViewDto() { 
							Document = new DocumentDto
							{
                                Url = o.Url,
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                         	ProductName = s2 == null ? "" : s2.Name.ToString(),
                         	ServiceName = s3 == null ? "" : s3.Name.ToString()
						 });


            var documentListDtos = await query.ToListAsync();

            return _documentsExcelExporter.ExportToFile(documentListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Documents)]
         public async Task<PagedResultDto<DocumentSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysRefRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.TenantId.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DocumentSysRefLookupTableDto>();
			foreach(var sysRef in sysRefList){
				lookupTableDtoList.Add(new DocumentSysRefLookupTableDto
				{
					Id = sysRef.Id,
					DisplayName = sysRef.TenantId?.ToString()
				});
			}

            return new PagedResultDto<DocumentSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Documents)]
         public async Task<PagedResultDto<DocumentProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DocumentProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new DocumentProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<DocumentProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Documents)]
         public async Task<PagedResultDto<DocumentServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_serviceRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var serviceList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DocumentServiceLookupTableDto>();
			foreach(var service in serviceList){
				lookupTableDtoList.Add(new DocumentServiceLookupTableDto
				{
					Id = service.Id,
					DisplayName = service.Name?.ToString()
				});
			}

            return new PagedResultDto<DocumentServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}