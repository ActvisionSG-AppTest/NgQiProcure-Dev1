using Abp.AspNetCore.Mvc.Authorization;
using QiProcureDemo.Authorization;
using QiProcureDemo.Storage;
using Abp.BackgroundJobs;

namespace QiProcureDemo.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}