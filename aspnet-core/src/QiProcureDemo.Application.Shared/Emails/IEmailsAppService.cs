using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Emails.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.Emails
{
    public interface IEmailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmailForViewDto>> GetAll(GetAllEmailsInput input);

        Task<GetEmailForViewDto> GetEmailForView(int id);

		Task<GetEmailForEditOutput> GetEmailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmailDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmailsToExcel(GetAllEmailsForExcelInput input);

		
		Task<PagedResultDto<EmailSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<EmailSysStatusLookupTableDto>> GetAllSysStatusForLookupTable(GetAllForLookupTableInput input);
		
    }
}