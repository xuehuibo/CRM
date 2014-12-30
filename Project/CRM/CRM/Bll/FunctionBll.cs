using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    /// <summary>
    /// 功能菜单数据读取类
    /// </summary>
    public class FunctionBll
    {
        /// <summary>
        /// 读取菜单
        /// </summary>
        /// <returns></returns>
        public static CMenu[] LoadMenu(IDal dal ,string groupCode)
        {
            int i;
            if (groupCode.Equals("*"))
            {
                //用户编码为*，从tfunction表取所有菜单
                var dt = dal.Select("select * from tFunction where Enabled=1 and FunType=1 order by ParentCode ,SerialNo", out i);
                return (from DataRow row in dt.Rows
                    select new CMenu
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        MenuCode = Convert.ToString(row["FunCode"]),
                        MenuName = Convert.ToString(row["FunName"]),
                        MenuCmd = Convert.ToString(row["FunCmd"]),
                        ParentCode = Convert.ToString(row["ParentCode"]),
                        SerialNo = Convert.ToInt16(row["SerialNo"])
                    }).ToArray();
            }
            else
            {
                //用户编码不为*，从tUserGroupFun取该用户菜单
                var dt = dal.Select("select a.Id,a.FunCode,a.FunName,a.FunCmd,a.ParentCode,a.SerialNo from tFunction a,tUserGroupFun b where a.FunCode=b.FunCode and  FunType=1 and GroupCode=@GroupCode",
                    out i,
                    dal.CreateParameter("@GroupCode", groupCode));
                return (from DataRow row in dt.Rows
                    select new CMenu
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        MenuCode = Convert.ToString(row["FunCode"]),
                        MenuName = Convert.ToString(row["FunName"]),
                        MenuCmd = Convert.ToString(row["FunCmd"]),
                        ParentCode = Convert.ToString(row["ParentCode"]),
                        SerialNo = Convert.ToInt16(row["SerialNo"])
                    }).ToArray();
            }

            
        }

        /// <summary>
        /// 读取菜单类别
        /// </summary>
        /// <returns></returns>
        public static CMenuCategory[] LoadMenuCategory(IDal dal, string groupCode)
        {
            int i;
            if (groupCode.Equals("*"))
            {
                //用户编码为*，从tfunction表取所有菜单
                var dt = dal.Select("select * from tFunction where Enabled=1 and FunType=0 order by ParentCode ,SerialNo", out i);
                return (from DataRow row in dt.Rows
                        select new CMenuCategory()
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            CategoryCode = Convert.ToString(row["FunCode"]),
                            CategoryName = Convert.ToString(row["FunName"]),
                            SerialNo = Convert.ToInt16(row["SerialNo"])
                        }).ToArray();
            }
            else
            {
                //用户编码不为*，从tUserGroupFun取该用户菜单
                var dt = dal.Select("select a.Id,a.FunCode,a.FunName,a.FunCmd,a.ParentCode,a.SerialNo from tFunction a,tUserGroupFun b where a.FunCode=b.FunCode and  FunType=0 and GroupCode=@GroupCode",
                    out i,
                    dal.CreateParameter("@GroupCode", groupCode));
                return (from DataRow row in dt.Rows
                        select new CMenuCategory
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            CategoryCode = Convert.ToString(row["FunCode"]),
                            CategoryName = Convert.ToString(row["FunName"]),
                            SerialNo = Convert.ToInt16(row["SerialNo"])
                        }).ToArray();
            }
        }
    }
}