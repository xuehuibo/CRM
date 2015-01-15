using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using CRM.Bll;
using CRM.Extend.HttpResponseMessages;
using CRM.Models;
using DAL;

namespace CRM.Controllers
{
    public class CheckInputerApiController : ApiController
    {
        // GET api/uniquecheckapi/5
        public CCheckResultModel Get(string dataSource, string value)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                try
                {
                    CCheckResultModel rst;
                    dal.Open();
                    switch (dataSource)
                    {
                        case "User.UserCode":
                            rst= UserBll.CheckUserCodeUnique(dal, value);
                            break;
                        case "Customer.CustomerCode":
                            rst = CustomerBll.CheckCustomerCodeUnique(dal, value);
                            break;
                        default:
                            throw new Exception("UniqueCheckInputer错误，没有找到指定DataSource");
                    }
                    
                    dal.Close();
                    return rst;
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "UniqueCheck.Get", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
            }
        }
    }
}
