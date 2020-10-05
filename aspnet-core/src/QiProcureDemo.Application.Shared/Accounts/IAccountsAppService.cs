using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Accounts.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.Accounts
{
    public interface IAccountsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAccountForViewDto>> GetAll(GetAllAccountsInput input);

        Task<GetAccountForViewDto> GetAccountForView(int id);

		Task<GetAccountForEditOutput> GetAccountForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAccountDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAccountsToExcel(GetAllAccountsForExcelInput input);

		
		Task<PagedResultDto<AccountTeamLookupTableDto>> GetAllTeamForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<AccountSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);
		
    }
}