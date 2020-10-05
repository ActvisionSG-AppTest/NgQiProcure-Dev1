using QiProcureDemo.SysRefs;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.SysStatuses.Exporting;
using QiProcureDemo.SysStatuses.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.SysStatuses
{
	[AbpAuthorize(AppPermissions.Pages_Administration_SysStatuses)]
    public class SysStatusesAppService : QiProcureDemoAppServiceBase, ISysStatusesAppService
    {
		 private readonly IRepository<SysStatus> _SysStatusRepository;
		 private readonly ISysStatusesExcelExporter _SysStatusesExcelExporter;
		 private readonly IRepository<SysRef,int> _lookup_sysRefRepository;
		 

		  public SysStatusesAppService(IRepository<SysStatus> SysStatusRepository, ISysStatusesExcelExporter SysStatusesExcelExporter , IRepository<SysRef, int> lookup_sysRefRepository) 
		  {
			_SysStatusRepository = SysStatusRepository;
			_SysStatusesExcelExporter = SysStatusesExcelExporter;
			_lookup_sysRefRepository = lookup_sysRefRepository;
		
		  }

		 public async Task<PagedResultDto<GetSysStatusForViewDto>> GetAll(GetAllSysStatusesInput input)
         {

            var filteredSysStatuses = _SysStatusRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinCodeFilter != null, e => e.Code >= input.MinCodeFilter)
                        .WhereIf(input.MaxCodeFilter != null, e => e.Code <= input.MaxCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefCodeFilter), e => e.SysRefFk.RefCode == input.RefCodeFilter);

            var pagedAndFilteredSysStatuses = filteredSysStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var SysStatuses = from o in pagedAndFilteredSysStatuses
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetSysStatusForViewDto() {
							SysStatus = new SysStatusDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id,
                                RefCode = o.SysRefFk.RefCode
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString()
						};

            var totalCount = await filteredSysStatuses.CountAsync();

            return new PagedResultDto<GetSysStatusForViewDto>(
                totalCount,
                await SysStatuses.ToListAsync()
            );
         }
		 
		 public async Task<GetSysStatusForViewDto> GetSysStatusForView(int id)
         {
            var SysStatus = await _SysStatusRepository.GetAsync(id);

            var output = new GetSysStatusForViewDto { SysStatus = ObjectMapper.Map<SysStatusDto>(SysStatus) };

		    if (output.SysStatus.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.SysStatus.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }
			
            return output;
         }
        public GetSysStatusForViewDto GetNextSysStatus(int? currentStatusCode, string refCode, string nextStatusName)
        {
            
            /* get approval status reference Id */
            var queryStatusRef = _lookup_sysRefRepository.GetAll()
               .Include(e => e.ReferenceTypeFk)
               .Where(e => e.ReferenceTypeFk != null && e.ReferenceTypeFk.Name == "Status")
               .Where(e => e.RefCode == refCode);

            var statusRefList = queryStatusRef.ToList();

            int statusRefId = 0;
            foreach (var statusRef in statusRefList)
            {
                statusRefId = statusRef.Id;
            }

            var filteredSysStatuses = _SysStatusRepository.GetAll()
                      .Include(e => e.SysRefFk)
                      .Where(e => e.SysRefId == statusRefId)
                      .Where(e => e.Code > currentStatusCode)
                      .WhereIf(!string.IsNullOrWhiteSpace(nextStatusName), e => e.Name.Contains(nextStatusName));

            var pagedAndFilteredSysStatuses = filteredSysStatuses
                .OrderBy("Code asc");

            var SysStatuses = from o in pagedAndFilteredSysStatuses
                              join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              select new GetSysStatusForViewDto()
                              {
                                  SysStatus = new SysStatusDto
                                  {
                                      Code = o.Code,
                                      Name = o.Name,
                                      Description = o.Description,
                                      Id = o.Id,
                                      RefCode = o.SysRefFk.RefCode
                                  },
                                  SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString()
                              };

            var SysStatusListDtos = SysStatuses.FirstOrDefault();
           
            return SysStatusListDtos;       
        }


        [AbpAuthorize(AppPermissions.Pages_Administration_SysStatuses_Edit)]
		 public async Task<GetSysStatusForEditOutput> GetSysStatusForEdit(EntityDto input)
         {
            var SysStatus = await _SysStatusRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSysStatusForEditOutput {SysStatus = ObjectMapper.Map<CreateOrEditSysStatusDto>(SysStatus)};

		    if (output.SysStatus.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.SysStatus.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSysStatusDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SysStatuses_Create)]
		 protected virtual async Task Create(CreateOrEditSysStatusDto input)
         {
            var SysStatus = ObjectMapper.Map<SysStatus>(input);

			
			if (AbpSession.TenantId != null)
			{
				SysStatus.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _SysStatusRepository.InsertAsync(SysStatus);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SysStatuses_Edit)]
		 protected virtual async Task Update(CreateOrEditSysStatusDto input)
         {
            var SysStatus = await _SysStatusRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, SysStatus);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SysStatuses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _SysStatusRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSysStatusesToExcel(GetAllSysStatusesForExcelInput input)
         {

            var filteredSysStatuses = _SysStatusRepository.GetAll()
                        .Include(e => e.SysRefFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinCodeFilter != null, e => e.Code >= input.MinCodeFilter)
                        .WhereIf(input.MaxCodeFilter != null, e => e.Code <= input.MaxCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

			var query = (from o in filteredSysStatuses
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetSysStatusForViewDto() { 
							SysStatus = new SysStatusDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString()
						 });


            var SysStatusListDtos = await query.ToListAsync();

            return _SysStatusesExcelExporter.ExportToFile(SysStatusListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_SysStatuses)]
         public async Task<PagedResultDto<SysStatusesysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysRefRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.TenantId != null ? e.TenantId.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<SysStatusesysRefLookupTableDto>();
			foreach(var sysRef in sysRefList){
				lookupTableDtoList.Add(new SysStatusesysRefLookupTableDto
				{
					Id = sysRef.Id,
					DisplayName = sysRef.TenantId?.ToString(),
                    RefCode = sysRef.RefCode.ToString()
                });
			}

            return new PagedResultDto<SysStatusesysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}