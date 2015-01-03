using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Http;
using CRM.Bll;
using CRM.Extend.HttpResponseMessages;
using CRM.Models;
using DAL;

namespace CRM.Controllers
{
    public class MenuApiController : ApiController
    {
        // GET api/menuapi
        public IEnumerable<CMenuCategory> Get(bool allMenu)
        {
            var authorityModel =
                (CAuthorityModel) HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,0))
            {
                try
                {
                    dal.Open();
                }
                catch
                {
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                var a= FunctionBll.LoadMenu(dal, allMenu ? "*" : authorityModel.GroupCode);
                return a;
            }
        }

        // POST api/menuapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/menuapi/5
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
