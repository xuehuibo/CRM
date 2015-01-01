using System;
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
            ViewBag.SignUser = HttpContext.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            return View();
        }

        [AllowAnonymous]
        public ViewResult Signin()
        {
            return View();
        }

        public ViewResult Signout()
        {
            var httpCookie = HttpContext.Request.Cookies["Token"];
            if (httpCookie != null)
            {
                HttpContext.Response.Cookies["Token"].Expires=DateTime.Now.AddDays(-1);
            }
            HttpContext.Session.RemoveAll();
            return View("Signin");
        }

        [AllowAnonymous]
        public PartialViewResult NoAuthority()
        {
            return PartialView();
        }
    }
}
