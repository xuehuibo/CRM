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
    public class CustContactApiController : ApiController
    {
        // GET api/contactapi
        public IEnumerable<CCustContact> Get(string customerCode,int page)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CCustContact[] custContacts;
                try
                {
                    dal.Open();
                    custContacts = CustContactBll.List(dal, customerCode, page);
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent =string.Format("{0}#{1}","CustContact.List",ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (custContacts == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return custContacts;
            }
        }

        // GET api/contactapi/5
        public CCustContact Get(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CCustContact custContact;
                try
                {
                    dal.Open();
                    custContact = CustContactBll.Get(dal, id);
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "CustContact.Get", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (custContact == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return custContact;
            }
        }

        // POST api/contactapi
        public CCustContact Post(CCustContact value)
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
                    ok = CustContactBll.Create(dal, value, string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("违反了 UNIQUE KEY 约束"))
                    {
                        throw new HttpResponseException(new PrimaryRepeatedMessge());
                    }
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "CustContact.Post", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogContent = string.Format("新建客户{0}-{1}", value.ContactCode, value.ContactName),
                        LogType = LogType.操作失败,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal, new CLog
                {
                    LogContent = string.Format("新建客户{0}-{1}", value.ContactCode, value.ContactName),
                    LogType = LogType.操作成功,
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                });
                dal.Close();
                return value;
            }
        }

        // PUT api/contactapi/5
        public CCustContact Put(int id, CCustContact value)
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
                    ok = CustContactBll.Update(dal, value, string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "CustContact.Put", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogContent = string.Format("修改客户{0}-{1}", value.ContactCode, value.ContactName),
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                    });
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                LogBll.Write(dal, new CLog
                {
                    LogContent = string.Format("修改客户{0}-{1}", value.ContactCode, value.ContactName),
                    LogType = LogType.操作成功,
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                });
                dal.Close();
                return value;
            }
        }
    }
}
