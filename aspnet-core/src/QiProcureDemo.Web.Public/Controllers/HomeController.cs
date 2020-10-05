using Microsoft.AspNetCore.Mvc;
using QiProcureDemo.Web.Controllers;

namespace QiProcureDemo.Web.Public.Controllers
{
    public class HomeController : QiProcureDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}