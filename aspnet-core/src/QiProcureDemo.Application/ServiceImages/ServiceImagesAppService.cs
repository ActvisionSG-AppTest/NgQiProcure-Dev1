using QiProcureDemo.Services;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ServiceImages.Exporting;
using QiProcureDemo.ServiceImages.Dtos;
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

namespace QiProcureDemo.ServiceImages
{
	[AbpAuthorize(AppPermissions.Pages_Management_ServiceImages)]
    public class ServiceImagesAppService : QiProcureDemoAppServiceBase, IServiceImagesAppService
    {

         private const int MaxServiceImagesBytes = 5242880; //5MB
         private readonly IRepository<ServiceImage> _serviceImageRepository;
		 private readonly IServiceImagesExcelExporter _serviceImagesExcelExporter;
		 private readonly IRepository<Service,int> _lookup_serviceRepository;
         private readonly ICacheManager _cacheManager;
         private readonly ITempFileCacheManager _tempFileCacheManager;

        public ServiceImagesAppService(IRepository<ServiceImage> serviceImageRepository, IServiceImagesExcelExporter serviceImagesExcelExporter , IRepository<Service, int> lookup_serviceRepository,
            ICacheManager cacheManager,
            ITempFileCacheManager tempFileCacheManager) 
		  {
			_serviceImageRepository = serviceImageRepository;
			_serviceImagesExcelExporter = serviceImagesExcelExporter;
			_lookup_serviceRepository = lookup_serviceRepository;
            _cacheManager = cacheManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

		 public async Task<PagedResultDto<GetServiceImageForViewDto>> GetAll(GetAllServiceImagesInput input)
         {
			
			var filteredServiceImages = _serviceImageRepository.GetAll()
						.Include( e => e.ServiceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter),  e => e.Url == input.UrlFilter)
						.WhereIf(input.IsMainFilter > -1,  e => (input.IsMainFilter == 1 && e.IsMain) || (input.IsMainFilter == 0 && !e.IsMain) )
                        .WhereIf(input.IsApprovedFilter > -1, e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter);

			var pagedAndFilteredServiceImages = filteredServiceImages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var serviceImages = from o in pagedAndFilteredServiceImages
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetServiceImageForViewDto() {
							ServiceImage = new ServiceImageDto
							{
                                Description = o.Description,
                                Url = o.Url,
                                IsMain = o.IsMain,
                                IsApproved = o.IsApproved,
                                Bytes = o.Bytes,
                                Id = o.Id
							},
                         	ServiceName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredServiceImages.CountAsync();

            return new PagedResultDto<GetServiceImageForViewDto>(
                totalCount,
                await serviceImages.ToListAsync()
            );
         }


        public async Task<PagedResultDto<GetServiceImageForViewDto>> GetServiceImagesByServiceId(int ServiceId)
        {

            var filteredServiceImages = _serviceImageRepository.GetAll()
                        .Include(e => e.ServiceFk)
                        .WhereIf(ServiceId != 0, e => e.ServiceId == ServiceId && e.IsDeleted == false);

            var pagedAndFilteredServiceImages = filteredServiceImages
                .OrderBy("id asc");

            var ServiceImages = from o in pagedAndFilteredServiceImages
                                join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new GetServiceImageForViewDto()
                                {
                                    ServiceImage = new ServiceImageDto
                                    {
                                        Description = o.Description,
                                        Url = o.Url,
                                        IsMain = o.IsMain,
                                        IsApproved = o.IsApproved,
                                        Id = o.Id,
                                        ServiceId = o.ServiceId,
                                        Bytes = o.Bytes,
                                        ImageBase64String = Convert.ToBase64String(o.Bytes)

                                    },
                                    ServiceName = s1 == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredServiceImages.CountAsync();

            return new PagedResultDto<GetServiceImageForViewDto>(
                totalCount,
                await ServiceImages.ToListAsync()
            );
        }
        public async Task<GetServiceImageForViewDto> GetServiceImageForView(int id)
         {
            var serviceImage = await _serviceImageRepository.GetAsync(id);

            var output = new GetServiceImageForViewDto { ServiceImage = ObjectMapper.Map<ServiceImageDto>(serviceImage) };

		    if (output.ServiceImage.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServiceImage.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceImages_Edit)]
		 public async Task<GetServiceImageForEditOutput> GetServiceImageForEdit(EntityDto input)
         {
            var serviceImage = await _serviceImageRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetServiceImageForEditOutput {ServiceImage = ObjectMapper.Map<CreateOrEditServiceImageDto>(serviceImage)};

		    if (output.ServiceImage.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServiceImage.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditServiceImageDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

        public async Task UpdateServiceImages(UpdateServiceImagesInput inputServiceImages)
        {

            byte[] byteArray;
            string _fileType;
            string[] _fileExtArr;
            string _fileExt = "jpeg";
            var imageBytes = _tempFileCacheManager.GetFile(inputServiceImages.FileToken);
            if (inputServiceImages.FileType != null)
            {
                _fileType = inputServiceImages.FileType;
                _fileExtArr = _fileType.Split("/");
                if (_fileExtArr.Length > 1)
                    _fileExt = _fileExtArr[1];
            }


            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + inputServiceImages.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (inputServiceImages.Width == 0 || inputServiceImages.Width > bmpImage.Width) ? bmpImage.Width : inputServiceImages.Width;
                var height = (inputServiceImages.Height == 0 || inputServiceImages.Height > bmpImage.Height) ? bmpImage.Height : inputServiceImages.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(inputServiceImages.X, inputServiceImages.Y, width, height), bmpImage.PixelFormat);

                using (var stream = new MemoryStream())
                {
                    bmCrop.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            if (byteArray.Length > MaxServiceImagesBytes)
            {
                throw new UserFriendlyException(L("Image_Warn_SizeLimit", AppConsts.MaxImageBytesUserFriendlyValue));
            }

            var inputDto = new ServiceImageDto
            {
                ServiceId = inputServiceImages.ServiceId,
                Url = inputServiceImages.FileToken + "." + _fileExt,
                Description = inputServiceImages.Description
            };

            var serviceImageDto = ObjectMapper.Map<ServiceImage>(inputDto);


            if (AbpSession.TenantId != null)
            {
                serviceImageDto.TenantId = (int?)AbpSession.TenantId;
            }

            serviceImageDto.Bytes = byteArray;

            await _serviceImageRepository.InsertAsync(serviceImageDto);
        }

        [AbpAuthorize(AppPermissions.Pages_Management_ServiceImages_Create)]
		 protected virtual async Task Create(CreateOrEditServiceImageDto input)
         {
            var serviceImage = ObjectMapper.Map<ServiceImage>(input);

			
			if (AbpSession.TenantId != null)
			{
				serviceImage.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _serviceImageRepository.InsertAsync(serviceImage);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceImages_Edit)]
		 protected virtual async Task Update(CreateOrEditServiceImageDto input)
         {
            var serviceImage = await _serviceImageRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, serviceImage);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceImages_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _serviceImageRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetServiceImagesToExcel(GetAllServiceImagesForExcelInput input)
         {
			
			var filteredServiceImages = _serviceImageRepository.GetAll()
						.Include( e => e.ServiceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter),  e => e.Url == input.UrlFilter)
						.WhereIf(input.IsMainFilter > -1,  e => (input.IsMainFilter == 1 && e.IsMain) || (input.IsMainFilter == 0 && !e.IsMain) )
                        .WhereIf(input.IsApprovedFilter > -1, e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter);

			var query = (from o in filteredServiceImages
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetServiceImageForViewDto() { 
							ServiceImage = new ServiceImageDto
							{
                                Description = o.Description,
                                Url = o.Url,
                                IsMain = o.IsMain,
                                IsApproved = o.IsApproved,
                                Bytes = o.Bytes,
                                Id = o.Id
							},
                         	ServiceName = s1 == null ? "" : s1.Name.ToString()
						 });


            var serviceImageListDtos = await query.ToListAsync();

            return _serviceImagesExcelExporter.ExportToFile(serviceImageListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Management_ServiceImages)]
         public async Task<PagedResultDto<ServiceImageServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_serviceRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var serviceList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ServiceImageServiceLookupTableDto>();
			foreach(var service in serviceList){
				lookupTableDtoList.Add(new ServiceImageServiceLookupTableDto
				{
					Id = service.Id,
					DisplayName = service.Name?.ToString()
				});
			}

            return new PagedResultDto<ServiceImageServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}