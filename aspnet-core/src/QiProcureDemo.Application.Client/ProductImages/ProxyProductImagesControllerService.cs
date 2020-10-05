using System;
using System.Threading.Tasks;
using Flurl.Http.Content;
using QiProcureDemo.ProductImages.Dto;

namespace QiProcureDemo.ProductImages
{
    public class ProxyProductImagesControllerService : ProxyControllerBase
    {
        public async Task<UploadProductImagesOutput> UploadProductImages(Action<CapturedMultipartContent> buildContent)
        {
            return await ApiClient
                .PostMultipartAsync<UploadProductImagesOutput>(GetEndpoint(nameof(UploadProductImages)), buildContent);
        }
    }
}