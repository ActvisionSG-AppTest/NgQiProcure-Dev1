using QiProcureDemo.Categories;
using QiProcureDemo.SysRefs;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Services.Exporting;
using QiProcureDemo.Services.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.SysRefs.Dtos.Custom;

namespace QiProcureDemo.Services
{
	[AbpAuthorize(AppPermissions.Pages_Management_Service_Services)]
    public class ServicesAppService : QiProcureDemoAppServiceBase, IServicesAppService
    {
		 private readonly IRepository<Service> _serviceRepository;
		 private readonly IServicesExcelExporter _servicesExcelExporter;
		 private readonly IRepository<Category,int> _lookup_categoryRepository;
		 private readonly IRepository<SysRef,int> _lookup_sysRefRepository;
		 

		  public ServicesAppService(IRepository<Service> serviceRepository, IServicesExcelExporter servicesExcelExporter , IRepository<Category, int> lookup_categoryRepository, IRepository<SysRef, int> lookup_sysRefRepository) 
		  {
			_serviceRepository = serviceRepository;
			_servicesExcelExporter = servicesExcelExporter;
			_lookup_categoryRepository = lookup_categoryRepository;
		_lookup_sysRefRepository = lookup_sysRefRepository;
		
		  }

		 public async Task<PagedResultDto<GetServiceForViewDto>> GetAll(GetAllServicesInput input)
         {
			
			var filteredServices = _serviceRepository.GetAll()
						.Include( e => e.CategoryFk)
						.Include( e => e.SysRefFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinDurationFilter != null, e => e.Duration >= input.MinDurationFilter)
						.WhereIf(input.MaxDurationFilter != null, e => e.Duration <= input.MaxDurationFilter)
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefRefCodeFilter), e => e.SysRefFk != null && e.SysRefFk.RefCode == input.SysRefRefCodeFilter);

			var pagedAndFilteredServices = filteredServices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var services = from o in pagedAndFilteredServices
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetServiceForViewDto() {
							Service = new ServiceDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Duration = o.Duration,
                                IsApproved = o.IsApproved,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	CategoryName = s1 == null ? "" : s1.Name.ToString(),
                         	SysRefRefCode = s2 == null ? "" : s2.RefCode.ToString()
						};

            var totalCount = await filteredServices.CountAsync();

            return new PagedResultDto<GetServiceForViewDto>(
                totalCount,
                await services.ToListAsync()
            );
         }

        public async Task<GetServiceForViewDto> GetServiceForView(int id)
         {
            var service = await _serviceRepository.GetAsync(id);

            var output = new GetServiceForViewDto { Service = ObjectMapper.Map<ServiceDto>(service) };

		    if (output.Service.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Service.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }

		    if (output.Service.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Service.SysRefId);
                output.SysRefRefCode = _lookupSysRef.RefCode.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_Service_Services_Edit)]
		 public async Task<GetServiceForEditOutput> GetServiceForEdit(EntityDto input)
         {
            var service = await _serviceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetServiceForEditOutput {Service = ObjectMapper.Map<CreateOrEditServiceDto>(service)};

		    if (output.Service.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Service.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }

		    if (output.Service.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Service.SysRefId);
                output.SysRefRefCode = _lookupSysRef.RefCode.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditServiceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_Service_Services_Create)]
		 protected virtual async Task Create(CreateOrEditServiceDto input)
         {
            var service = ObjectMapper.Map<Service>(input);

			
			if (AbpSession.TenantId != null)
			{
				service.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _serviceRepository.InsertAsync(service);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_Service_Services_Edit)]
		 protected virtual async Task Update(CreateOrEditServiceDto input)
         {
            var service = await _serviceRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, service);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_Service_Services_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _serviceRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetServicesToExcel(GetAllServicesForExcelInput input)
         {
			
			var filteredServices = _serviceRepository.GetAll()
						.Include( e => e.CategoryFk)
						.Include( e => e.SysRefFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinDurationFilter != null, e => e.Duration >= input.MinDurationFilter)
						.WhereIf(input.MaxDurationFilter != null, e => e.Duration <= input.MaxDurationFilter)
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefRefCodeFilter), e => e.SysRefFk != null && e.SysRefFk.RefCode == input.SysRefRefCodeFilter);

			var query = (from o in filteredServices
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetServiceForViewDto() { 
							Service = new ServiceDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Duration = o.Duration,
                                IsApproved = o.IsApproved,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Id = o.Id
							},
                         	CategoryName = s1 == null ? "" : s1.Name.ToString(),
                         	SysRefRefCode = s2 == null ? "" : s2.RefCode.ToString()
						 });


            var serviceListDtos = await query.ToListAsync();

            return _servicesExcelExporter.ExportToFile(serviceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Management_Service_Services)]
         public async Task<PagedResultDto<ServiceCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_categoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ServiceCategoryLookupTableDto>();
			foreach(var category in categoryList){
				lookupTableDtoList.Add(new ServiceCategoryLookupTableDto
				{
					Id = category.Id,
					DisplayName = category.Name?.ToString()
				});
			}

            return new PagedResultDto<ServiceCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Management_Service_Services)]
         public async Task<PagedResultDto<ServiceSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysRefRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.RefCode.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ServiceSysRefLookupTableDto>();
			foreach(var sysRef in sysRefList){
				lookupTableDtoList.Add(new ServiceSysRefLookupTableDto
				{
					Id = sysRef.Id,
					DisplayName = sysRef.RefCode?.ToString()
				});
			}

            return new PagedResultDto<ServiceSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task<PagedResultDto<ServiceSysRefLookupTableDto>> GetSysRefByRefType(GetAllSysRefsInput input)
        {
            var query = _lookup_sysRefRepository.GetAll()
                .Include(e => e.ReferenceTypeFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeNameFilter), e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == input.ReferenceTypeNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.RefCodeFilter), e => e.RefCode == input.RefCodeFilter);

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<ServiceSysRefLookupTableDto>();
            foreach (var sysRef in sysRefList)
            {
                lookupTableDtoList.Add(new ServiceSysRefLookupTableDto
                {
                    Id = sysRef.Id,
                    DisplayName = sysRef.RefCode?.ToString()
                });
            }

            return new PagedResultDto<ServiceSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}