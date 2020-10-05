using QiProcureDemo.Teams;
using QiProcureDemo.SysStatuses;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Accounts.Exporting;
using QiProcureDemo.Accounts.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.Accounts
{
	[AbpAuthorize(AppPermissions.Pages_Accounts)]
    public class AccountsAppService : QiProcureDemoAppServiceBase, IAccountsAppService
    {
		 private readonly IRepository<Account> _accountRepository;
		 private readonly IAccountsExcelExporter _accountsExcelExporter;
		 private readonly IRepository<Team,int> _lookup_teamRepository;
		 private readonly IRepository<SysStatus,int> _lookup_sysStatusRepository;
		 

		  public AccountsAppService(IRepository<Account> accountRepository, IAccountsExcelExporter accountsExcelExporter , IRepository<Team, int> lookup_teamRepository, IRepository<SysStatus, int> lookup_sysStatusRepository) 
		  {
			_accountRepository = accountRepository;
			_accountsExcelExporter = accountsExcelExporter;
			_lookup_teamRepository = lookup_teamRepository;
		_lookup_sysStatusRepository = lookup_sysStatusRepository;
		
		  }

		 public async Task<PagedResultDto<GetAccountForViewDto>> GetAll(GetAllAccountsInput input)
         {
			
			var filteredAccounts = _accountRepository.GetAll()
						.Include( e => e.TeamFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.UserName.Contains(input.Filter) || e.Password.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsPersonalFilter > -1,  e => (input.IsPersonalFilter == 1 && e.IsPersonal) || (input.IsPersonalFilter == 0 && !e.IsPersonal) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),  e => e.UserName == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PasswordFilter),  e => e.Password == input.PasswordFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var pagedAndFilteredAccounts = filteredAccounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var accounts = from o in pagedAndFilteredAccounts
                         join o1 in _lookup_teamRepository.GetAll() on o.TeamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetAccountForViewDto() {
							Account = new AccountDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                IsPersonal = o.IsPersonal,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Code = o.Code,
                                Email = o.Email,
                                UserName = o.UserName,
                                Password = o.Password,
                                Id = o.Id
							},
                         	TeamName = s1 == null ? "" : s1.Name.ToString(),
                         	SysStatusName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredAccounts.CountAsync();

            return new PagedResultDto<GetAccountForViewDto>(
                totalCount,
                await accounts.ToListAsync()
            );
         }
		 
		 public async Task<GetAccountForViewDto> GetAccountForView(int id)
         {
            var account = await _accountRepository.GetAsync(id);

            var output = new GetAccountForViewDto { Account = ObjectMapper.Map<AccountDto>(account) };

		    if (output.Account.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.Account.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

		    if (output.Account.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Account.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Accounts_Edit)]
		 public async Task<GetAccountForEditOutput> GetAccountForEdit(EntityDto input)
         {
            var account = await _accountRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAccountForEditOutput {Account = ObjectMapper.Map<CreateOrEditAccountDto>(account)};

		    if (output.Account.TeamId != null)
            {
                var _lookupTeam = await _lookup_teamRepository.FirstOrDefaultAsync((int)output.Account.TeamId);
                output.TeamName = _lookupTeam.Name.ToString();
            }

		    if (output.Account.SysStatusId != null)
            {
                var _lookupSysStatus = await _lookup_sysStatusRepository.FirstOrDefaultAsync((int)output.Account.SysStatusId);
                output.SysStatusName = _lookupSysStatus.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAccountDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Accounts_Create)]
		 protected virtual async Task Create(CreateOrEditAccountDto input)
         {
            var account = ObjectMapper.Map<Account>(input);

			
			if (AbpSession.TenantId != null)
			{
				account.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _accountRepository.InsertAsync(account);
         }

		 [AbpAuthorize(AppPermissions.Pages_Accounts_Edit)]
		 protected virtual async Task Update(CreateOrEditAccountDto input)
         {
            var account = await _accountRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, account);
         }

		 [AbpAuthorize(AppPermissions.Pages_Accounts_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _accountRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAccountsToExcel(GetAllAccountsForExcelInput input)
         {
			
			var filteredAccounts = _accountRepository.GetAll()
						.Include( e => e.TeamFk)
						.Include( e => e.SysStatusFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Remark.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.UserName.Contains(input.Filter) || e.Password.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsPersonalFilter > -1,  e => (input.IsPersonalFilter == 1 && e.IsPersonal) || (input.IsPersonalFilter == 0 && !e.IsPersonal) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter),  e => e.Remark == input.RemarkFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),  e => e.UserName == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PasswordFilter),  e => e.Password == input.PasswordFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TeamNameFilter), e => e.TeamFk != null && e.TeamFk.Name == input.TeamNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SysStatusNameFilter), e => e.SysStatusFk != null && e.SysStatusFk.Name == input.SysStatusNameFilter);

			var query = (from o in filteredAccounts
                         join o1 in _lookup_teamRepository.GetAll() on o.TeamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_sysStatusRepository.GetAll() on o.SysStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetAccountForViewDto() { 
							Account = new AccountDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                IsPersonal = o.IsPersonal,
                                IsActive = o.IsActive,
                                Remark = o.Remark,
                                Code = o.Code,
                                Email = o.Email,
                                UserName = o.UserName,
                                Password = o.Password,
                                Id = o.Id
							},
                         	TeamName = s1 == null ? "" : s1.Name.ToString(),
                         	SysStatusName = s2 == null ? "" : s2.Name.ToString()
						 });


            var accountListDtos = await query.ToListAsync();

            return _accountsExcelExporter.ExportToFile(accountListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Accounts)]
         public async Task<PagedResultDto<AccountTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_teamRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var teamList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AccountTeamLookupTableDto>();
			foreach(var team in teamList){
				lookupTableDtoList.Add(new AccountTeamLookupTableDto
				{
					Id = team.Id,
					DisplayName = team.Name?.ToString()
				});
			}

            return new PagedResultDto<AccountTeamLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Accounts)]
         public async Task<PagedResultDto<AccountSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_sysStatusRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.Name != null ? e.Name.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var sysStatusList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AccountSysStatusLookupTableDto>();
			foreach(var sysStatus in sysStatusList){
				lookupTableDtoList.Add(new AccountSysStatusLookupTableDto
				{
					Id = sysStatus.Id,
					DisplayName = sysStatus.Name?.ToString()
				});
			}

            return new PagedResultDto<AccountSysStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}