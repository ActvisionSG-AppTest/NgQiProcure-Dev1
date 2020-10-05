using QiProcureDemo.SysRefs;
using QiProcureDemo.SysStatuses;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Emails.Exporting;
using QiProcureDemo.Emails.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.Emails
{
	[AbpAuthorize(AppPermissions.Pages_Emails)]
    public class EmailsAppService : QiProcureDemoAppServiceBase, IEmailsAppService
    {
		 private readonly IRepository<Email> _emailRepository;
		 private readonly IEmailsExcelExporter _emailsExcelExporter;
		 private readonly IRepository<SysRef,int> _lookup_sysRefRepository;
		 private readonly IRepository<SysStatus,int> _lookup_sysStatusRepository;
		 

		  public EmailsAppService(IRepository<Email> emailRepository, IEmailsExcelExporter emailsExcelExporter , IRepository<SysRef, int> lookup_sysRefRepository, IRepository<SysStatus, int> lookup_sysStatusRepository) 
		  {
			_emailRepository = emailRepository;
			_emailsExcelExporter = emailsExcelExporter;
			_lookup_sysRefRepository = lookup_sysRefRepository;
		_lookup_sysStatusRepository = lookup_sysStatusRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmailForViewDto>> GetAll(GetAllEmailsInput input)
         {
			
			var filteredEmails = _emailRepository.GetAll()
						.Include( e => e.SysRefFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EmailFrom.Contains(input.Filter) || e.EmailTo.Contains(input.Filter) || e.EmailCC.Contains(input.Filter) || e.EmailBCC.Contains(input.Filter) || e.Subject.Contains(input.Filter) || e.Body.Contains(input.Filter))
						.WhereIf(input.MinReferenceIdFilter != null, e => e.ReferenceId >= input.MinReferenceIdFilter)
						.WhereIf(input.MaxReferenceIdFilter != null, e => e.ReferenceId <= input.MaxReferenceIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFromFilter),  e => e.EmailFrom == input.EmailFromFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailToFilter),  e => e.EmailTo == input.EmailToFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailCCFilter),  e => e.EmailCC == input.EmailCCFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailBCCFilter),  e => e.EmailBCC == input.EmailBCCFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter),  e => e.Subject == input.SubjectFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BodyFilter),  e => e.Body == input.BodyFilter)
						.WhereIf(input.MinRequestDateFilter != null, e => e.RequestDate >= input.MinRequestDateFilter)
						.WhereIf(input.MaxRequestDateFilter != null, e => e.RequestDate <= input.MaxRequestDateFilter)
						.WhereIf(input.MinSentDateFilter != null, e => e.SentDate >= input.MinSentDateFilter)
						.WhereIf(input.MaxSentDateFilter != null, e => e.SentDate <= input.MaxSentDateFilter)
/*						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefTenantIdFilter), e => e.SysRefFk != null && e.SysRefFk.TenantId == input.SysRefTenantIdFilter)
*/						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var pagedAndFilteredEmails = filteredEmails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var emails = from o in pagedAndFilteredEmails
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmailForViewDto() {
							Email = new EmailDto
							{
                                ReferenceId = o.ReferenceId,
                                EmailFrom = o.EmailFrom,
                                EmailTo = o.EmailTo,
                                EmailCC = o.EmailCC,
                                EmailBCC = o.EmailBCC,
                                Subject = o.Subject,
                                Body = o.Body,
                                RequestDate = o.RequestDate,
                                SentDate = o.SentDate,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                         	SysStatusName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredEmails.CountAsync();

            return new PagedResultDto<GetEmailForViewDto>(
                totalCount,
                await emails.ToListAsync()
            );
         }
		 
		 public async Task<GetEmailForViewDto> GetEmailForView(int id)
         {
            var email = await _emailRepository.GetAsync(id);

            var output = new GetEmailForViewDto { Email = ObjectMapper.Map<EmailDto>(email) };

		    if (output.Email.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Email.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.Email.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Email.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Emails_Edit)]
		 public async Task<GetEmailForEditOutput> GetEmailForEdit(EntityDto input)
         {
            var email = await _emailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmailForEditOutput {Email = ObjectMapper.Map<CreateOrEditEmailDto>(email)};

		    if (output.Email.SysRefId != null)
            {
                var _lookupSysRef = await _lookup_sysRefRepository.FirstOrDefaultAsync((int)output.Email.SysRefId);
                output.SysRefTenantId = _lookupSysRef.TenantId.ToString();
            }

		    if (output.Email.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Email.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Emails_Create)]
		 protected virtual async Task Create(CreateOrEditEmailDto input)
         {
            var email = ObjectMapper.Map<Email>(input);

			
			if (AbpSession.TenantId != null)
			{
				email.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _emailRepository.InsertAsync(email);
         }

		 [AbpAuthorize(AppPermissions.Pages_Emails_Edit)]
		 protected virtual async Task Update(CreateOrEditEmailDto input)
         {
            var email = await _emailRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, email);
         }

		 [AbpAuthorize(AppPermissions.Pages_Emails_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _emailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmailsToExcel(GetAllEmailsForExcelInput input)
         {
			
			var filteredEmails = _emailRepository.GetAll()
						.Include( e => e.SysRefFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EmailFrom.Contains(input.Filter) || e.EmailTo.Contains(input.Filter) || e.EmailCC.Contains(input.Filter) || e.EmailBCC.Contains(input.Filter) || e.Subject.Contains(input.Filter) || e.Body.Contains(input.Filter))
						.WhereIf(input.MinReferenceIdFilter != null, e => e.ReferenceId >= input.MinReferenceIdFilter)
						.WhereIf(input.MaxReferenceIdFilter != null, e => e.ReferenceId <= input.MaxReferenceIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFromFilter),  e => e.EmailFrom == input.EmailFromFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailToFilter),  e => e.EmailTo == input.EmailToFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailCCFilter),  e => e.EmailCC == input.EmailCCFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailBCCFilter),  e => e.EmailBCC == input.EmailBCCFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter),  e => e.Subject == input.SubjectFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BodyFilter),  e => e.Body == input.BodyFilter)
						.WhereIf(input.MinRequestDateFilter != null, e => e.RequestDate >= input.MinRequestDateFilter)
						.WhereIf(input.MaxRequestDateFilter != null, e => e.RequestDate <= input.MaxRequestDateFilter)
						.WhereIf(input.MinSentDateFilter != null, e => e.SentDate >= input.MinSentDateFilter)
						.WhereIf(input.MaxSentDateFilter != null, e => e.SentDate <= input.MaxSentDateFilter)
/*						.WhereIf(!string.IsNullOrWhiteSpace(input.SysRefTenantIdFilter), e => e.SysRefFk != null && e.SysRefFk.TenantId == input.SysRefTenantIdFilter)
*/						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var query = (from o in filteredEmails
                         join o1 in _lookup_sysRefRepository.GetAll() on o.SysRefId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmailForViewDto() { 
							Email = new EmailDto
							{
                                ReferenceId = o.ReferenceId,
                                EmailFrom = o.EmailFrom,
                                EmailTo = o.EmailTo,
                                EmailCC = o.EmailCC,
                                EmailBCC = o.EmailBCC,
                                Subject = o.Subject,
                                Body = o.Body,
                                RequestDate = o.RequestDate,
                                SentDate = o.SentDate,
                                Id = o.Id
							},
                         	SysRefTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                         	SysStatusName = s2 == null ? "" : s2.Name.ToString()
						 });


            var emailListDtos = await query.ToListAsync();

            return _emailsExcelExporter.ExportToFile(emailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Emails)]
         public async Task<PagedResultDto<EmailSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysRefRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.TenantId != null ? e.TenantId.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysRefList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmailSysRefLookupTableDto>();
			foreach(var sysRef in sysRefList){
				lookupTableDtoList.Add(new EmailSysRefLookupTableDto
				{
					Id = sysRef.Id,
					DisplayName = sysRef.TenantId?.ToString()
				});
			}

            return new PagedResultDto<EmailSysRefLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Emails)]
         public async Task<PagedResultDto<EmailSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmailSysStatusLookupTableDto>();
			foreach(var sysStatus in sysStatusList){
				lookupTableDtoList.Add(new EmailSysStatusLookupTableDto
				{
					Id = sysStatus.Id,
					DisplayName = sysStatus.Name?.ToString()
				});
			}

            return new PagedResultDto<EmailSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}