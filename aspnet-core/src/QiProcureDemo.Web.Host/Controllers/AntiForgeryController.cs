using Microsoft.AspNetCore.Antiforgery;

namespace QiProcureDemo.Web.Controllers
{
    public class AntiForgeryController : QiProcureDemoControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
