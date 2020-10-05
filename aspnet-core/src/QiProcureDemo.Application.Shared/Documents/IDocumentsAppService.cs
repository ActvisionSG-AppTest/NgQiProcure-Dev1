using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.Documents.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Documents
{
    public interface IDocumentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDocumentForViewDto>> GetAll(GetAllDocumentsInput input);

        Task<GetDocumentForViewDto> GetDocumentForView(int id);

		Task<GetDocumentForEditOutput> GetDocumentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDocumentDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDocumentsToExcel(GetAllDocumentsForExcelInput input);

		Task UpdateDocuments(UpdateDocumentsInput inputDocuments);

		Task<PagedResultDto<DocumentSysRefLookupTableDto>> GetAllSysRefForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<DocumentProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<DocumentServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input);
		
    }
}