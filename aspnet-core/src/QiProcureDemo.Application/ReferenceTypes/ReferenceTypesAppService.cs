

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.ReferenceTypes.Exporting;
using QiProcureDemo.ReferenceTypes.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.ReferenceTypes
{
	[AbpAuthorize(AppPermissions.Pages_Administration_Reference_ReferenceTypes)]
    public class ReferenceTypesAppService : QiProcureDemoAppServiceBase, IReferenceTypesAppService
    {
		 private readonly IRepository<ReferenceType> _referenceTypeRepository;
		 private readonly IReferenceTypesExcelExporter _referenceTypesExcelExporter;
		 

		  public ReferenceTypesAppService(IRepository<ReferenceType> referenceTypeRepository, IReferenceTypesExcelExporter referenceTypesExcelExporter ) 
		  {
			_referenceTypeRepository = referenceTypeRepository;
			_referenceTypesExcelExporter = referenceTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetReferenceTypeForViewDto>> GetAll(GetAllReferenceTypesInput input)
         {
			
			var filteredReferenceTypes = _referenceTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.ReferenceTypeCode.Contains(input.Filter) || e.ReferenceTypeGroup.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeCodeFilter),  e => e.ReferenceTypeCode == input.ReferenceTypeCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeGroupFilter),  e => e.ReferenceTypeGroup == input.ReferenceTypeGroupFilter);

			var pagedAndFilteredReferenceTypes = filteredReferenceTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var referenceTypes = from o in pagedAndFilteredReferenceTypes
                         select new GetReferenceTypeForViewDto() {
							ReferenceType = new ReferenceTypeDto
							{
                                Name = o.Name,
                                ReferenceTypeCode = o.ReferenceTypeCode,
                                ReferenceTypeGroup = o.ReferenceTypeGroup,
                                Id = o.Id
							}
						};

            var totalCount = await filteredReferenceTypes.CountAsync();

            return new PagedResultDto<GetReferenceTypeForViewDto>(
                totalCount,
                await referenceTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetReferenceTypeForViewDto> GetReferenceTypeForView(int id)
         {
            var referenceType = await _referenceTypeRepository.GetAsync(id);

            var output = new GetReferenceTypeForViewDto { ReferenceType = ObjectMapper.Map<ReferenceTypeDto>(referenceType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Edit)]
		 public async Task<GetReferenceTypeForEditOutput> GetReferenceTypeForEdit(EntityDto input)
         {
            var referenceType = await _referenceTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetReferenceTypeForEditOutput {ReferenceType = ObjectMapper.Map<CreateOrEditReferenceTypeDto>(referenceType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditReferenceTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Create)]
		 protected virtual async Task Create(CreateOrEditReferenceTypeDto input)
         {
            var referenceType = ObjectMapper.Map<ReferenceType>(input);

			
			if (AbpSession.TenantId != null)
			{
				referenceType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _referenceTypeRepository.InsertAsync(referenceType);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditReferenceTypeDto input)
         {
            var referenceType = await _referenceTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, referenceType);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _referenceTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetReferenceTypesToExcel(GetAllReferenceTypesForExcelInput input)
         {
			
			var filteredReferenceTypes = _referenceTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.ReferenceTypeCode.Contains(input.Filter) || e.ReferenceTypeGroup.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeCodeFilter),  e => e.ReferenceTypeCode == input.ReferenceTypeCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTypeGroupFilter),  e => e.ReferenceTypeGroup == input.ReferenceTypeGroupFilter);

			var query = (from o in filteredReferenceTypes
                         select new GetReferenceTypeForViewDto() { 
							ReferenceType = new ReferenceTypeDto
							{
                                Name = o.Name,
                                ReferenceTypeCode = o.ReferenceTypeCode,
                                ReferenceTypeGroup = o.ReferenceTypeGroup,
                                Id = o.Id
							}
						 });


            var referenceTypeListDtos = await query.ToListAsync();

            return _referenceTypesExcelExporter.ExportToFile(referenceTypeListDtos);
         }


    }
}