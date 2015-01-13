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
    public class CustomerApiController : ApiController
    {
        // GET api/customerapi
        public IEnumerable<CCustomer> Get(string customerCode,string customerName,string owner,int page)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CCustomer[] customers;
                try
                {
                    dal.Open();
                    customers = CustomerBll.List(dal, customerCode, customerName, owner,page);
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "Customer.List", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }

                if (customers == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return customers;
            }
        }

        // GET api/customerapi/5
        public CCustomer Get(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CCustomer customer;
                try
                {
                    dal.Open();
                    customer = CustomerBll.Get(dal, id);
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "Customer.Get", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }

                if (customer == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return customer;
            }
        }

        // POST api/customerapi
        public CCustomer Post(CCustomer value)
        {
            var user = (CSign) HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (
                var dal =
                    DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0)
                )
            {
                bool ok;
                try
                {
                    dal.Open();
                    ok = CustomerBll.Create(dal, value, string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "Customer.Post", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogContent = string.Format("新建客户{0}-{1}", value.CustomerCode, value.CustomerName),
                        LogType = LogType.操作失败,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal, new CLog
                {
                    LogContent = string.Format("新建客户{0}-{1}", value.CustomerCode, value.CustomerName),
                    LogType = LogType.操作成功,
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                });
                dal.Close();
                return value;
            }
        }

        // PUT api/customerapi/5
        public CCustomer Put(int id, CCustomer value)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                try
                {
                    dal.Open();
                    ok = CustomerBll.Update(dal, value, string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "Customer.Put", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogContent = string.Format("修改客户{0}-{1}", value.CustomerCode, value.CustomerName),
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                    });
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                LogBll.Write(dal, new CLog
                {
                    LogContent = string.Format("修改客户{0}-{1}", value.CustomerCode, value.CustomerName),
                    LogType = LogType.操作成功,
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                });
                dal.Close();
                return value;
            }
        }

    }
}
