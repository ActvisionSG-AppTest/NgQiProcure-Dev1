using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QiProcureDemo.ProductImages.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Authorization.Users.Profile.Dto;

namespace QiProcureDemo.ProductImages
{
    public interface IProductImagesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductImageForViewDto>> GetAll(GetAllProductImagesInput input);

		Task<PagedResultDto<GetProductImageForViewDto>> GetProductImagesByProductId(int ProductId);
		
		Task<GetProductImageForViewDto> GetProductImageForView(int id);

		Task<GetProductImageForEditOutput> GetProductImageForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProductImageDto input);

		Task UpdateProductImages(UpdateProductImagesInput inputProductImages);

		Task Delete(EntityDto input);

		Task<FileDto> GetProductImagesToExcel(GetAllProductImagesForExcelInput input);

		
		Task<PagedResultDto<ProductImageProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

		//Task UpdateProfilePicture(UpdateProfilePictureInput input);

	}
}