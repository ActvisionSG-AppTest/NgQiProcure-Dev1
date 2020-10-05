using QiProcureDemo.ReferenceTypes;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.SysRefs.Exporting;
using QiProcureDemo.SysRefs.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.SysRefs.Dtos.Custom;

namespace QiProcureDemo.SysRefs
{
	[AbpAuthorize(AppPermissions.Pages_Administration_Reference_SysRefs)]
    public class SysRefsAppService : QiProcureDemoAppServiceBase, ISysRefsAppService
    {
		 private readonly IRepository<SysRef> _sysRefRepository;
		 private readonly ISysRefsExcelExporter _sysRefsExcelExporter;
		 private readonly IRepository<ReferenceType,int> _lookup_referenceTypeRepository;
		 

		  public SysRefsAppService(IRepository<SysRef> sysRefRepository, ISysRefsExcelExporter sysRefsExcelExporter , IRepository<ReferenceType, int> lookup_referenceTypeRepository) 
		  {
			_sysRefRepository = sysRefRepository;
			_sysRefsExcelExporter = sysRefsExcelExporter;
			_lookup_referenceTypeRepository = lookup_referenceTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetSysRefForViewDto>> GetAll(GetAllSysRefsInput input)
         {
			
			var filteredSysRefs = _sysRefRepository.GetAll()
						.Include( e => e.ReferenceTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.RefCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.RefCodeFilter),  e => e.RefCode == input.RefCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.ReferenceTypeIdFilter != null, e => e.ReferenceTypeId == input.ReferenceTypeIdFilter)
						.WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
						.WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeNameFilter), e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == input.ReferenceTypeNameFilter);

			var pagedAndFilteredSysRefs = filteredSysRefs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var sysRefs = from o in pagedAndFilteredSysRefs
                         join o1 in _lookup_referenceTypeRepository.GetAll() on o.ReferenceTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetSysRefForViewDto() {
							SysRef = new SysRefDto
							{
                                RefCode = o.RefCode,
                                Description = o.Description,
                                OrderNumber = o.OrderNumber,
                                Id = o.Id
							},
                         	ReferenceTypeName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredSysRefs.CountAsync();

            return new PagedResultDto<GetSysRefForViewDto>(
                totalCount,
                await sysRefs.ToListAsync()
            );
         }
		 
		 public async Task<GetSysRefForViewDto> GetSysRefForView(int id)
         {
            var sysRef = await _sysRefRepository.GetAsync(id);

            var output = new GetSysRefForViewDto { SysRef = ObjectMapper.Map<SysRefDto>(sysRef) };

		    if (output.SysRef.ReferenceTypeId != null)
            {
                var _lookupReferenceType = await _lookup_referenceTypeRepository.FirstOrDefaultAsync((int)output.SysRef.ReferenceTypeId);
                output.ReferenceTypeName = _lookupReferenceType.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_SysRefs_Edit)]
		 public async Task<GetSysRefForEditOutput> GetSysRefForEdit(EntityDto input)
         {
            var sysRef = await _sysRefRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSysRefForEditOutput {SysRef = ObjectMapper.Map<CreateOrEditSysRefDto>(sysRef)};

		    if (output.SysRef.ReferenceTypeId != null)
            {
                var _lookupReferenceType = await _lookup_referenceTypeRepository.FirstOrDefaultAsync((int)output.SysRef.ReferenceTypeId);
                output.ReferenceTypeName = _lookupReferenceType.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSysRefDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_SysRefs_Create)]
		 protected virtual async Task Create(CreateOrEditSysRefDto input)
         {
            var sysRef = ObjectMapper.Map<SysRef>(input);

			
			if (AbpSession.TenantId != null)
			{
				sysRef.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _sysRefRepository.InsertAsync(sysRef);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_SysRefs_Edit)]
		 protected virtual async Task Update(CreateOrEditSysRefDto input)
         {
            var sysRef = await _sysRefRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, sysRef);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_SysRefs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _sysRefRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSysRefsToExcel(GetAllSysRefsForExcelInput input)
         {
			
			var filteredSysRefs = _sysRefRepository.GetAll()
						.Include( e => e.ReferenceTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.RefCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.RefCodeFilter),  e => e.RefCode == input.RefCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
						.WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeNameFilter), e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == input.ReferenceTypeNameFilter);

			var query = (from o in filteredSysRefs
                         join o1 in _lookup_referenceTypeRepository.GetAll() on o.ReferenceTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetSysRefForViewDto() { 
							SysRef = new SysRefDto
							{
                                RefCode = o.RefCode,
                                Description = o.Description,
                                OrderNumber = o.OrderNumber,
                                Id = o.Id
							},
                         	ReferenceTypeName = s1 == null ? "" : s1.Name.ToString()
						 });


            var sysRefListDtos = await query.ToListAsync();

            return _sysRefsExcelExporter.ExportToFile(sysRefListDtos);
         }

        [AbpAuthorize(AppPermissions.Pages_Administration_Reference_SysRefs)]
        public async Task<PagedResultDto<SysRefReferenceTypeLookupTableDto>> GetAllReferenceTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_referenceTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var referenceTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SysRefReferenceTypeLookupTableDto>();
            foreach (var referenceType in referenceTypeList)
            {
                lookupTableDtoList.Add(new SysRefReferenceTypeLookupTableDto
                {
                    Id = referenceType.Id,
                    DisplayName = referenceType.Name?.ToString()
                });
            }

            return new PagedResultDto<SysRefReferenceTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}