using System.Web.Mvc;
using CRM.Attribute;

namespace CRM.Controllers
{
    [Authorization]
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Register()
        {
            ViewBag.MenuCategory = "客户管理";
            ViewBag.Menu = "客户登记";
            return View();
        }

    }
}
