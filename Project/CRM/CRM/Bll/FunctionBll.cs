using System;
using System.Data;
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
        public static CMenuCategory[] LoadMenu(IDal dal ,string groupCode)
        {
            DataTable dt;
            int i;
            if (groupCode.Equals("*"))
            {
                //用户组编码为*，从tfunction表取所有菜单
                dt = dal.Select("select * from tFunction where Enabled=1 order by ParentCode ,SerialNo", out i);
            }
            else
            {
                //用户组编码不为*，从tUserGroupFun取该用户菜单
                dt = dal.Select("select a.Id,a.FunCode,a.FunName,a.FunCmd,a.ParentCode,a.FunType,a.SerialNo from tFunction a,tUserGroupFun b where a.FunCode=b.FunCode and GroupCode=@GroupCode",
                    out i,
                    dal.CreateParameter("@GroupCode", groupCode));
            }
            return (from DataRow category in dt.Rows
                where category["FunType"].ToString() == "0"
                select new CMenuCategory
                {
                    Id = Convert.ToInt16(category["Id"]),
                    CategoryCode = Convert.ToString(category["FunCode"]),
                    CategoryName = Convert.ToString(category["FunName"]),
                    SerialNo = Convert.ToInt16(category["SerialNo"]),
                    Menus = (from DataRow menu in dt.Rows
                             where menu["FunType"].ToString() == "1" && Convert.ToString(menu["ParentCode"])==Convert.ToString(category["FunCode"])
                        select new CMenu
                        {
                            Id = Convert.ToInt16(menu["Id"]),
                            MenuCode = Convert.ToString(menu["FunCode"]),
                            MenuName = Convert.ToString(menu["FunName"]),
                            MenuCmd = Convert.ToString(menu["FunCmd"]),
                            ParentCode = Convert.ToString(menu["ParentCode"]),
                            SerialNo = Convert.ToInt16(menu["SerialNo"])
                        }).ToArray()
                }).ToArray();
        }
    }
}