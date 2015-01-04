using System.Web.Mvc;

namespace CRM.Controllers
{
    public class ConfigController : Controller
    {
        //
        // GET: /Config/

        public ViewResult Index()
        {
            ViewBag.MenuCategory = "参数设置";
            ViewBag.Menu = "部门设置";
            return View();
        }
    }
}
