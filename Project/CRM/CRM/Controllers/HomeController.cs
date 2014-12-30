using System.Configuration;
using System.Web.Mvc;
using CRM.Attribute;

namespace CRM.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ViewResult Index()
        {
            ViewBag.Title = ConfigurationManager.AppSettings["BrandName"];
            return View();
        }

        [AllowAnonymous]
        public ViewResult Signin()
        {
            return View();
        }

    }
}
