using QiProcureDemo.Services;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ServicePrices.Exporting;
using QiProcureDemo.ServicePrices.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.ServicePrices
{
	[AbpAuthorize(AppPermissions.Pages_Management_ServicePrices)]
    public class ServicePricesAppService : QiProcureDemoAppServiceBase, IServicePricesAppService
    {
		 private readonly IRepository<ServicePrice> _servicePriceRepository;
		 private readonly IServicePricesExcelExporter _servicePricesExcelExporter;
		 private readonly IRepository<Service,int> _lookup_serviceRepository;
		 

		  public ServicePricesAppService(IRepository<ServicePrice> servicePriceRepository, IServicePricesExcelExporter servicePricesExcelExporter , IRepository<Service, int> lookup_serviceRepository) 
		  {
			_servicePriceRepository = servicePriceRepository;
			_servicePricesExcelExporter = servicePricesExcelExporter;
			_lookup_serviceRepository = lookup_serviceRepository;
		
		  }

		 public async Task<PagedResultDto<GetServicePriceForViewDto>> GetAll(GetAllServicePricesInput input)
         {
			
			var filteredServicePrices = _servicePriceRepository.GetAll()
						.Include( e => e.ServiceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.MinValidityFilter != null, e => e.Validity >= input.MinValidityFilter)
						.WhereIf(input.MaxValidityFilter != null, e => e.Validity <= input.MaxValidityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter);

			var pagedAndFilteredServicePrices = filteredServicePrices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var servicePrices = from o in pagedAndFilteredServicePrices
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetServicePriceForViewDto() {
							ServicePrice = new ServicePriceDto
							{
                                Price = o.Price,
                                Validity = o.Validity,
                                Id = o.Id
							},
                         	ServiceName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredServicePrices.CountAsync();

            return new PagedResultDto<GetServicePriceForViewDto>(
                totalCount,
                await servicePrices.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetServicePriceForViewDto>> GetByServiceId(GetAllServicePricesInput input)
        {

            var filteredServicePrices = _servicePriceRepository.GetAll()
                        .Include(e => e.ServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .Where(e => e.ServiceId == input.ServiceIdFilter);

            var pagedAndFilteredServicePrices = filteredServicePrices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var servicePrices = from o in pagedAndFilteredServicePrices
                                join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new GetServicePriceForViewDto()
                                {
                                    ServicePrice = new ServicePriceDto
                                    {
                                        Price = o.Price,
                                        Validity = o.Validity,
                                        Id = o.Id
                                    },
                                    ServiceName = s1 == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredServicePrices.CountAsync();

            return new PagedResultDto<GetServicePriceForViewDto>(
                totalCount,
                await servicePrices.ToListAsync()
            );
        }

        public async Task<GetServicePriceForViewDto> GetServicePriceForView(int id)
         {
            var servicePrice = await _servicePriceRepository.GetAsync(id);

            var output = new GetServicePriceForViewDto { ServicePrice = ObjectMapper.Map<ServicePriceDto>(servicePrice) };

		    if (output.ServicePrice.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServicePrice.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Management_ServicePrices_Edit)]
		 public async Task<GetServicePriceForEditOutput> GetServicePriceForEdit(EntityDto input)
         {
            var servicePrice = await _servicePriceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetServicePriceForEditOutput {ServicePrice = ObjectMapper.Map<CreateOrEditServicePriceDto>(servicePrice)};

		    if (output.ServicePrice.ServiceId != null)
            {
                var _lookupService = await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServicePrice.ServiceId);
                output.ServiceName = _lookupService.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditServicePriceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServicePrices_Create)]
		 protected virtual async Task Create(CreateOrEditServicePriceDto input)
         {
            var servicePrice = ObjectMapper.Map<ServicePrice>(input);

			
			if (AbpSession.TenantId != null)
			{
				servicePrice.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _servicePriceRepository.InsertAsync(servicePrice);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServicePrices_Edit)]
		 protected virtual async Task Update(CreateOrEditServicePriceDto input)
         {
            var servicePrice = await _servicePriceRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, servicePrice);
         }

		 [AbpAuthorize(AppPermissions.Pages_Management_ServicePrices_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _servicePriceRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetServicePricesToExcel(GetAllServicePricesForExcelInput input)
         {
			
			var filteredServicePrices = _servicePriceRepository.GetAll()
						.Include( e => e.ServiceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.MinValidityFilter != null, e => e.Validity >= input.MinValidityFilter)
						.WhereIf(input.MaxValidityFilter != null, e => e.Validity <= input.MaxValidityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ServiceNameFilter), e => e.ServiceFk != null && e.ServiceFk.Name == input.ServiceNameFilter);

			var query = (from o in filteredServicePrices
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetServicePriceForViewDto() { 
							ServicePrice = new ServicePriceDto
							{
                                Price = o.Price,
                                Validity = o.Validity,
                                Id = o.Id
							},
                         	ServiceName = s1 == null ? "" : s1.Name.ToString()
						 });


            var servicePriceListDtos = await query.ToListAsync();

            return _servicePricesExcelExporter.ExportToFile(servicePriceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Management_ServicePrices)]
         public async Task<PagedResultDto<ServicePriceServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_serviceRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var serviceList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ServicePriceServiceLookupTableDto>();
			foreach(var service in serviceList){
				lookupTableDtoList.Add(new ServicePriceServiceLookupTableDto
				{
					Id = service.Id,
					DisplayName = service.Name?.ToString()
				});
			}

            return new PagedResultDto<ServicePriceServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}