﻿using System;
using System.Security.Cryptography;
using System.Text;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    /// <summary>
    /// 权限控制操作类
    /// </summary>
    public class SignBll
    {
        /// <summary>
        /// 通过用户名密码登录
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="authorityModel"></param>
        /// <returns></returns>
        public static bool Signin(IDal dal,CSign authorityModel)
        {
            int i;
            var pwd = MD5.Create().ComputeHash(Encoding.Default.GetBytes(authorityModel.UserCode + authorityModel.UPwd));
            var dt = dal.Select("select * from tUser where UPassword=@UPassword", out i,
                dal.CreateParameter("@UPassword", pwd));
            if (i == 0)
            {
                return false;
            }
            authorityModel.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            authorityModel.UserName = Convert.ToString(dt.Rows[0]["UserName"]).Trim();
            authorityModel.GroupCode = Convert.IsDBNull(dt.Rows[0]["GroupCode"])
                ? null
                : Convert.ToString(dt.Rows[0]["GroupCode"]).Trim();
            authorityModel.DeptCode = Convert.IsDBNull(dt.Rows[0]["DeptCode"])
                ? null
                : Convert.ToString(dt.Rows[0]["DeptCode"]).Trim();
            return true;
        }

        /// <summary>
        /// 通过token登录
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="token"></param>
        /// <param name="authorityModel"></param>
        /// <returns></returns>
        public static bool Signin(IDal dal,string token,CSign authorityModel)
        {
            int i;
            var tk = MD5.Create().ComputeHash(Encoding.Default.GetBytes(token));
            var dt = dal.Select(" select * from tUser where Token=@Token ", out i,
                dal.CreateParameter("@Token", tk));
            if (i == 0)
            {
                return false;
            }
            authorityModel.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            authorityModel.UserName = Convert.ToString(dt.Rows[0]["UserName"]).Trim();
            authorityModel.UserCode = Convert.ToString(dt.Rows[0]["UserCode"]).Trim();
            authorityModel.GroupCode = Convert.IsDBNull(dt.Rows[0]["GroupCode"])
                ? null
                : Convert.ToString(dt.Rows[0]["GroupCode"]).Trim();
            authorityModel.DeptCode = Convert.IsDBNull(dt.Rows[0]["DeptCode"])
                ? null
                : Convert.ToString(dt.Rows[0]["DeptCode"]).Trim();
            return true;
        }

        /// <summary>
        /// 更新Token
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="token"></param>
        /// <param name="userCode"></param>
        public static void UpdateToken(IDal dal, string token, string userCode)
        {
            var vlu = MD5.Create().ComputeHash(Encoding.Default.GetBytes(token));
            int i;
            dal.Execute(" update tUser set Token=@Token where UserCode=@UserCode ", out i,
                dal.CreateParameter("@Token",vlu),
                dal.CreateParameter("@UserCode",userCode));
        }

        /// <summary>
        /// 取消Token
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userCode"></param>
        public static void DropToken(IDal dal, string userCode)
        {
            int i;
            dal.Execute(" update tUser set Token=null where UserCode=@UserCode ", out i,
                dal.CreateParameter("@UserCode", userCode));
        }
    }
}