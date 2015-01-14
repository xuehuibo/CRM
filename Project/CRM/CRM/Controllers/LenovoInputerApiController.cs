using System;
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
    public class LenovoInputerApiController : ApiController
    {
        // GET api/userselecterapi
        public IEnumerable<CLenovoInputOption> Get(string dataSource,string condition)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CLenovoInputOption[] options;
                try
                {
                    dal.Open();
                    switch (dataSource)
                    {
                        case "User":
                            options = UserBll.GetLenovoInputOption(dal, condition);
                            break;
                        case "Dept":
                            options = DeptBll.GetLenovoInputOption(dal, condition);
                            break;
                        case "UserGroup":
                            options = UserGroupBll.GetLenovoInputOption(dal, condition);
                            break;
                        default:
                            throw new HttpResponseException(new SystemExceptionMessage());
                    }
                    
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "LenovoInputer.List", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (options == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return options;
            }
        }
    }
}
