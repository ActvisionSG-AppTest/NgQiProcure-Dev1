
using Abp.AspNetCore.Mvc.Authorization;
using QiProcureDemo.Storage;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QiProcureDemo.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProductImagesController : ProductImagesControllerBase
    {
        // GET: /<controller>/
        public ProductImagesController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}
