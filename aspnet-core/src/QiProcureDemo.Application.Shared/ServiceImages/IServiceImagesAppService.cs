using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ServiceImages.Dtos;
using QiProcureDemo.Dto;

namespace QiProcureDemo.ServiceImages
{
    public interface IServiceImagesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetServiceImageForViewDto>> GetAll(GetAllServiceImagesInput input);

        Task<GetServiceImageForViewDto> GetServiceImageForView(int id);

		Task<GetServiceImageForEditOutput> GetServiceImageForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditServiceImageDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetServiceImagesToExcel(GetAllServiceImagesForExcelInput input);

		
		Task<PagedResultDto<ServiceImageServiceLookupTableDto>> GetAllServiceForLookupTable(GetAllForLookupTableInput input);
		
    }
}