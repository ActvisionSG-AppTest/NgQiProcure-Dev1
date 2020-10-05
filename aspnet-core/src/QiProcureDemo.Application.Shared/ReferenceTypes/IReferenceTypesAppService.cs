using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ReferenceTypes.Dtos;
using QiProcureDemo.Dto;


namespace QiProcureDemo.ReferenceTypes
{
    public interface IReferenceTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetReferenceTypeForViewDto>> GetAll(GetAllReferenceTypesInput input);

        Task<GetReferenceTypeForViewDto> GetReferenceTypeForView(int id);

		Task<GetReferenceTypeForEditOutput> GetReferenceTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditReferenceTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetReferenceTypesToExcel(GetAllReferenceTypesForExcelInput input);

		
    }
}