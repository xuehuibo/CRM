﻿using System;
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
    public class DeptApiController : ApiController
    {
        // GET api/deptapi
        public IEnumerable<CDept> Get()
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CDept[] depts;
                try
                {
                    dal.Open();
                    depts = DeptBll.List(dal);
                    dal.Close();
                }
                catch(Exception ex)
                {
                    LogBll.Write(new CLog
                    {
                        LogDate = DateTime.Now,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = ex.Message,
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                
                if (depts == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return depts;
            }
        }

        // GET api/deptapi/5
        public CDept Get(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,0))
            {
                CDept dept;
                try
                {
                    dal.Open();
                    dept = DeptBll.Get(dal, id);
                    dal.Close();
                }
                catch(Exception ex)
                {
                    LogBll.Write(new CLog
                    {
                        LogDate = DateTime.Now,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = ex.Message,
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }

                if (dept == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return dept;
            }
        }

        // POST api/deptapi
        public CDept Post(CDept value)
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
                    value.BuildUser = string.Format("{0}-{1}", user.UserCode, user.UserName);
                    value.EditUser = string.Format("{0}-{1}", user.UserCode, user.UserName);
                    ok = DeptBll.Create(dal, value);
                }
                catch(Exception ex)
                {
                    LogBll.Write(new CLog
                    {
                        LogDate = DateTime.Now,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = ex.Message,
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogContent = string.Format("新建部门{0}-{1}", value.DeptCode, value.DeptName),
                        LogDate = DateTime.Now,
                        LogType = LogType.操作失败,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogContent = string.Format("新建部门{0}-{1}",value.DeptCode,value.DeptName),
                    LogDate = DateTime.Now,
                    LogType = LogType.操作成功 ,
                    LogUser = string.Format("{0}-{1}",user.UserCode,user.UserName)
                });
                dal.Close();
                return value;
            }
        }

        // PUT api/deptapi/5
        public CDept Put(int id, CDept value)
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
                    value.EditUser = string.Format("{0}-{1}", user.UserCode, user.UserName);
                    ok = DeptBll.Update(dal, value);
                }
                catch(Exception ex)
                {
                    LogBll.Write(new CLog
                    {
                        LogDate = DateTime.Now,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = ex.Message,
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogContent = string.Format("修改部门{0}-{1}", value.DeptCode, value.DeptName),
                        LogDate = DateTime.Now,
                        LogType = LogType.操作失败,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                    });
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                LogBll.Write(dal, new CLog
                {
                    LogContent = string.Format("修改部门{0}-{1}", value.DeptCode, value.DeptName),
                    LogDate = DateTime.Now,
                    LogType = LogType.操作成功,
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                });
                dal.Close();
                return value;
            }
        }

        // DELETE api/deptapi/5
        public void Delete(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                CDept hisDept;
                try
                {
                    dal.Open();
                    ok = DeptBll.Delete(dal, id,out hisDept);
                }
                catch(Exception ex)
                {
                    LogBll.Write(new CLog
                    {
                        LogDate = DateTime.Now,
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = ex.Message,
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    if (hisDept != null)
                    {
                        LogBll.Write(dal, new CLog
                        {
                            LogContent = string.Format("删除部门{0}-{1}", hisDept.DeptCode, hisDept.DeptName),
                            LogDate = DateTime.Now,
                            LogType = LogType.操作失败,
                            LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                        });
                    }
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                LogBll.Write(dal, new CLog
                {
                    LogContent = string.Format("删除部门{0}-{1}", hisDept.DeptCode, hisDept.DeptName),
                    LogDate = DateTime.Now,
                    LogType = LogType.操作成功 ,
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName)
                });
                dal.Close();
            }
        }
    }
}
