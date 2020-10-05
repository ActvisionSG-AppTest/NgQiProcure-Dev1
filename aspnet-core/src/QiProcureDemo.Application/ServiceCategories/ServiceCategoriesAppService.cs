using QiProcureDemo.Services;
using QiProcureDemo.Categories;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ServiceCategories.Exporting;
using QiProcureDemo.ServiceCategories.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp;

namespace QiProcureDemo.ServiceCategories
{
	[AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories)]
    public class ServiceCategoriesAppService : QiProcureDemoAppServiceBase, IServiceCategoriesAppService
    {
		 private readonly IRepository<ServiceCategory> _serviceCategoryRepository;
		 private readonly IServiceCategoriesExcelExporter _serviceCategoriesExcelExporter;
		 private readonly IRepository<Service,int> _lookup_serviceRepository;
		 private readonly IRepository<Category,int> _lookup_categoryRepository;
		 

		  public ServiceCategoriesAppService(IRepository<ServiceCategory> serviceCategoryRepository, IServiceCategoriesExcelExporter serviceCategoriesExcelExporter , IRepository<Service, int> lookup_serviceRepository, IRepository<Category, int> lookup_categoryRepository) 
		  {
			_serviceCategoryRepository = serviceCategoryRepository;
			_serviceCategoriesExcelExporter = serviceCategoriesExcelExporter;
			_lookup_serviceRepository = lookup_serviceRepository;
		_lookup_categoryRepository = lookup_categoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetServiceCategoryForViewDto>> GetAll(GetAllServiceCategoriesInput input)
         {
			
			var filteredServiceCategories = _serviceCategoryRepository.GetAll()
						.Include( e => e.ServiceFk)
						.Include( e => e.CategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

			var pagedAndFilteredServiceCategories = filteredServiceCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var serviceCategories = from o in pagedAndFilteredServiceCategories
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetServiceCategoryForViewDto() {
							ServiceCategory = new ServiceCategoryDto
							{
                                Id = o.Id
							},
                         	ServiceName = s1 == null ? "" : s1.Name.ToString(),
                         	CategoryName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredServiceCategories.CountAsync();

            return new PagedResultDto<GetServiceCategoryForViewDto>(
                totalCount,
                await serviceCategories.ToListAsync()
            );
         }


        public async Task<PagedResultDto<GetServiceCategoryForViewDto>> GetServiceCategory(int ServiceId, int CategoryId)
        {

            var filteredServiceCategories = _serviceCategoryRepository.GetAll()
                        .Include(e => e.ServiceFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(ServiceId != 0, e => e.ServiceId == ServiceId && e.CategoryId == CategoryId);


            var ServiceCategories = from o in filteredServiceCategories
                                    join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new GetServiceCategoryForViewDto()
                                    {
                                        ServiceCategory = new ServiceCategoryDto
                                        {
                                            Id = o.Id
                                        },
                                        ServiceName = s1 == null ? "" : s1.Name.ToString(),
                                        CategoryName = s2 == null ? "" : s2.Name.ToString()

                                    };

            var totalCount = await filteredServiceCategories.CountAsync();

            return new PagedResultDto<GetServiceCategoryForViewDto>(
                totalCount,
                await ServiceCategories.ToListAsync()
            );
        }

        public List<NameValue<string>> GetServiceCategoriesNameValue(int ServiceId)
        {

            var filteredServiceCategories = _serviceCategoryRepository.GetAll()
                        .Include(e => e.ServiceFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(ServiceId != 0, e => e.ServiceId == ServiceId);

            var pagedAndFilteredServiceCategories = filteredServiceCategories
                  .OrderBy("Id asc");

            var serviceCategories = (from o in filteredServiceCategories
                                     join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     select new GetServiceCategoryForViewDto()
                                     {
                                         ServiceCategory = new ServiceCategoryDto
                                         {
                                             CategoryId = o.CategoryId
                                         },

                                         CategoryName = s2 == null ? "" : s2.Name.ToString()
                                     }).Distinct();

            var serviceCategoryList = new List<NameValue<string>> { };

            foreach (var serviceCategory in serviceCategories)
            {
                serviceCategoryList.Add(new NameValue { Name = serviceCategory.CategoryName, Value = serviceCategory.ServiceCategory.CategoryId.ToString() });
            }

            return serviceCategoryList;
        }


        [AbpAuthorize(AppPermissions.Pages_Management_ProductCategories_Delete)]
        public virtual async Task sendAndGetSelectedCategories(List<NameValueService> selectedCategories)
        {

            int i = 0;

            foreach (var category in selectedCategories)
            {
                var filteredServiceCategories = _serviceCategoryRepository.GetAll()
                                           .Include(e => e.ServiceFk)
                                           .Include(e => e.CategoryFk)
                                           .Where(e => (e.ServiceId == Convert.ToInt32(category.ServiceId)) && e.CategoryId == Convert.ToInt32(category.Value));

                var totalCount = await filteredServiceCategories.CountAsync();
                if (totalCount == 0)
                {


                    var serviceCategory = new ServiceCategory
                    {
                        ServiceId = Convert.ToInt32(category.ServiceId),
                        CategoryId = (Convert.ToInt32(category.Value))
                    };
                    if (AbpSession.TenantId != null)
                    {
                        serviceCategory.TenantId = (int?)AbpSession.TenantId;
                    }
                    await _serviceCategoryRepository.InsertAsync(serviceCategory);
                }

            }
        }

        public class NameValueService
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string ServiceId { get; set; }

        }
        public async Task<GetServiceCategoryForViewDto> GetServiceCategoryForView(int id)
         {
            var serviceCategory = await _serviceCategoryRepository.GetAsync(id);

            var output = new GetServiceCategoryForViewDto { ServiceCategory = ObjectMapper.Map<ServiceCategoryDto>(serviceCategory) };

		    if (output.ServiceCategory.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServiceCategory.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }

		    if (output.ServiceCategory.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.ServiceCategory.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories_Edit)]
		 public async Task<GetServiceCategoryForEditOutput> GetServiceCategoryForEdit(EntityDto input)
         {
            var serviceCategory = await _serviceCategoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetServiceCategoryForEditOutput {ServiceCategory = ObjectMapper.Map<CreateOrEditServiceCategoryDto>(serviceCategory)};

		    if (output.ServiceCategory.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServiceCategory.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }

		    if (output.ServiceCategory.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.ServiceCategory.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditServiceCategoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories_Create)]
		 protected virtual async Task Create(CreateOrEditServiceCategoryDto input)
         {
            var serviceCategory = ObjectMapper.Map<ServiceCategory>(input);

			
			if (AbpSession.TenantId != null)
			{
				serviceCategory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _serviceCategoryRepository.InsertAsync(serviceCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditServiceCategoryDto input)
         {
            var serviceCategory = await _serviceCategoryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, serviceCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _serviceCategoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetServiceCategoriesToExcel(GetAllServiceCategoriesForExcelInput input)
         {
			
			var filteredServiceCategories = _serviceCategoryRepository.GetAll()
						.Include( e => e.ServiceFk)
						.Include( e => e.CategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

			var query = (from o in filteredServiceCategories
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetServiceCategoryForViewDto() { 
							ServiceCategory = new ServiceCategoryDto
							{
                                Id = o.Id
							},
                         	ServiceName = s1 == null ? "" : s1.Name.ToString(),
                         	CategoryName = s2 == null ? "" : s2.Name.ToString()
						 });


            var serviceCategoryListDtos = await query.ToListAsync();

            return _serviceCategoriesExcelExporter.ExportToFile(serviceCategoryListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories)]
         public async Task<PagedResultDto<ServiceCategoryServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_serviceRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var serviceList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ServiceCategoryServiceLookupTableDto>();
			foreach(var service in serviceList){
				lookupTableDtoList.Add(new ServiceCategoryServiceLookupTableDto
				{
					Id = service.Id,
					DisplayName = service.Name?.ToString()
				});
			}

            return new PagedResultDto<ServiceCategoryServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Management_ServiceCategories)]
         public async Task<PagedResultDto<ServiceCategoryCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_categoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ServiceCategoryCategoryLookupTableDto>();
			foreach(var category in categoryList){
				lookupTableDtoList.Add(new ServiceCategoryCategoryLookupTableDto
				{
					Id = category.Id,
					DisplayName = category.Name?.ToString()
				});
			}

            return new PagedResultDto<ServiceCategoryCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}