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
    public class MenuApiController : ApiController
    {
        // GET api/menuapi
        public IEnumerable<CMenuCategory> Get()
        {
            
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,0))
            {
                var user =
                    (CSign) HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
                try
                {
                    dal.Open();
                    if (user == null)
                    {
                        var httpCookie = HttpContext.Current.Request.Cookies["Token"];
                        if (httpCookie != null)
                        {
                            //存在Token，进行Token登录

                            if (SignBll.Signin(dal, httpCookie.Values["User"], httpCookie.Values["Value"],
                                user))
                            {
                                HttpContext.Current.Session.Add(ConfigurationManager.AppSettings["AuthSaveKey"],
                                    user);
                                //更新Token
                                var token = Guid.NewGuid().ToString();
                                HttpContext.Current.Response.Cookies["Token"].Values["User"] = user.UserCode;
                                HttpContext.Current.Response.Cookies["Token"].Values["Value"] = token;
                                HttpContext.Current.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(30);
                                SignBll.UpdateToken(dal, token, user.UserCode);
                            }
                            else
                            {
                                throw new HttpResponseException(new SiginFailureMessage());
                            }
                        }
                        else
                        {
                            throw new HttpResponseException(new SiginFailureMessage());
                        }

                    }
                    var menus = FunctionBll.LoadMenu(dal, user.GroupCode);
                    if (menus == null)
                    {
                        throw new HttpResponseException(new DataNotFoundMessage());
                    }
                    return menus;
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user==null?string.Empty:user.UserCode, user==null?string.Empty:user.UserName),
                        LogContent = string.Format("{0}#{1}", "Menu.List", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                finally
                {
                    dal.Close();
                }
            }
        }

    }
}
