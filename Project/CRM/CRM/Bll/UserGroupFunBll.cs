using System;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class UserGroupFunBll
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <returns></returns>
        public static CUserGroupFun[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tUserGroupFun ", out i);
            return (from DataRow row in dt.Rows
                select new CUserGroupFun
                {
                    Id = Convert.ToInt16(row["Id"]),
                    GroupCode = Convert.ToString(row["GroupCode"]).Trim(),
                    FunCode = Convert.ToString(row["FunCode"]).Trim(),
                    Queriable = Convert.ToBoolean(row["Queriable"]),
                    Changable = Convert.ToBoolean(row["Changable"]),
                    Deletable = Convert.ToBoolean(row["Deletable"]),
                    Checkable = Convert.ToBoolean(row["Checkable"]),
                    Creatable = Convert.ToBoolean(row["Creatable"])
                }).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public static CUserGroupFun[] List(IDal dal, string groupCode)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tUserGroupFun WHERE GroupCode=@GroupCode ", out i,
                dal.CreateParameter("@GroupCode",groupCode));
            return (from DataRow row in dt.Rows
                    select new CUserGroupFun
                    {
                        Id = Convert.ToInt16(row["Id"]),
                        GroupCode = Convert.ToString(row["GroupCode"]).Trim(),
                        FunCode = Convert.ToString(row["FunCode"]).Trim(),
                        Queriable = Convert.ToBoolean(row["Queriable"]),
                        Changable = Convert.ToBoolean(row["Changable"]),
                        Deletable = Convert.ToBoolean(row["Deletable"]),
                        Checkable = Convert.ToBoolean(row["Checkable"]),
                        Creatable = Convert.ToBoolean(row["Creatable"])
                    }).ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroupFun"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CUserGroupFun userGroupFun,string user)
        {
            int i;
            dal.Execute("INSERT INTO tUserGroupFun( GroupCode ,FunCode ,Queriable ,Creatable ,Changable ,Deletable ,Checkable,BuildUser,EditUser) VALUES  ( @GroupCode , @FunCode ,@Queriable ,@Creatable ,@Changable ,@Deletable @Checkable,@BuildUser,@EditUser )", out i,
                dal.CreateParameter("@GroupCode",userGroupFun.GroupCode),
                dal.CreateParameter("@FunCode",userGroupFun.FunCode),
                dal.CreateParameter("@Queriable",userGroupFun.Queriable),
                dal.CreateParameter("@Creatable",userGroupFun.Creatable),
                dal.CreateParameter("@Changable",userGroupFun.Changable),
                dal.CreateParameter("@Deletable",userGroupFun.Deletable),
                dal.CreateParameter("@Checkable",userGroupFun.Checkable),
                dal.CreateParameter("@BuildUser",user),
                dal.CreateParameter("@EditUser",user));
            if (i == 0) return false;
            var dt = dal.Select("SELECT Id FROM tUserGroupFun WHERE GroupCode=@GroupCode AND FunCode=@FunCode", out i,
                dal.CreateParameter("@GroupCode", userGroupFun.GroupCode),
                dal.CreateParameter("@FunCode", userGroupFun.FunCode));
            if (i == 0) return false;
            userGroupFun.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroupFun"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CUserGroupFun userGroupFun,string user)
        {
            int i;
            dal.Execute("UPDATE tUserGroupFun SET Changable=@Changable,Checkable=@Checkable,Deletable=@Deletable,Queriable=@Queriable,Creatable=@Creatable WHERE Id=@Id",out i,
                dal.CreateParameter("@Changable",userGroupFun.Changable),
                dal.CreateParameter("@Checkable",userGroupFun.Checkable),
                dal.CreateParameter("@Deletable",userGroupFun.Deletable),
                dal.CreateParameter("@Queriable",userGroupFun.Queriable),
                dal.CreateParameter("@Creatable",userGroupFun.Creatable),
                dal.CreateParameter("@Id",userGroupFun.Id)
                );
            return i != 0 || Create(dal, userGroupFun, user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(IDal dal, int id)
        {
            int i;
            dal.Execute("DELETE FROM tUserGroupFun WHERE Id=@Id",out i,
                dal.CreateParameter("@Id",id));
            return i==1;
        }
    }
}