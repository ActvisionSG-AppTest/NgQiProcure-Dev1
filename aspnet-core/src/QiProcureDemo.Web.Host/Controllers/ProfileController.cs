using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using QiProcureDemo.Authorization.Users.Profile;
using QiProcureDemo.Storage;

namespace QiProcureDemo.Web.Controllers
{
    [Authorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService) :
            base(tempFileCacheManager, profileAppService)
        {
        }
    }
}