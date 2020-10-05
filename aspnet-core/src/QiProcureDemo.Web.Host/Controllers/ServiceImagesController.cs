
using Abp.AspNetCore.Mvc.Authorization;
using QiProcureDemo.Storage;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QiProcureDemo.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ServiceImagesController : ServiceImagesControllerBase
    {
        // GET: /<controller>/
        public ServiceImagesController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}
