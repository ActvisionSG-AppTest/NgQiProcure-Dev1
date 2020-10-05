using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.SysRefs.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.SysRefs.Dtos.Custom;

namespace QiProcureDemo.SysRefs
{
    public interface ISysRefsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSysRefForViewDto>> GetAll(GetAllSysRefsInput input);

        Task<GetSysRefForViewDto> GetSysRefForView(int id);

		Task<GetSysRefForEditOutput> GetSysRefForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSysRefDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSysRefsToExcel(GetAllSysRefsForExcelInput input);

		
    }
}