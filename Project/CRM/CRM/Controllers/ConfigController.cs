using System.Configuration;
using System.Web.Mvc;
using CRM.Attribute;

namespace CRM.Controllers
{
    [Authorization]
    public class ConfigController : Controller
    {
        //
        // GET: /Config/

        public PartialViewResult DeptManage()
        {
            ViewBag.Title = ConfigurationManager.AppSettings["BrandName"];
            ViewBag.MenuCategory = "参数设置";
            ViewBag.Menu = "部门设置";
            return PartialView();
        }

        public PartialViewResult UserGroupManage()
        {
            ViewBag.MenuCategory = "参数设置";
            ViewBag.Menu = "用户组";
            return PartialView();
        }

        public PartialViewResult UserManage()
        {
            ViewBag.MenuCategory = "参数设置";
            ViewBag.Menu = "人员管理";
            return PartialView();
        }
    }
}
