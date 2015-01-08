using System;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class UserGroupBll
    {
        /// <summary>
        /// 列表用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <returns></returns>
        public static CUserGroup[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tUserGroup Order by GroupCode", out i);
            if (i == 0)
            {
                return null;
            }
            return (from DataRow row in dt.Rows
                select new CUserGroup
                {
                    Id = Convert.ToInt16(row["Id"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"]),
                    GroupType = (GroupType)Convert.ToInt16(row["GroupType"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.ToString(row["EditUser"])
                }).ToArray();
        }

        /// <summary>
        /// 获取指定用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CUserGroup Get(IDal dal,int id)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tUserGroup WHERE Id=@Id", out i,
                dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUserGroup
                {
                    Id = Convert.ToInt16(row["Id"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"]),
                    GroupType = (GroupType)Convert.ToInt16(row["GroupType"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.ToString(row["EditUser"])
                }).First();
        }

        /// <summary>
        /// 新建用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CUserGroup userGroup)
        {
            int i;
            dal.Execute(
                "INSERT INTO tUserGroup( GroupCode ,GroupName ,GroupType ,BuildUser ,EditUser) VALUES  ( @GroupCode , @GroupName,@GroupType ,@BuildUser,@EditUser)",
                out i,
                dal.CreateParameter("@GroupCode", userGroup.GroupCode),
                dal.CreateParameter("@GroupName", userGroup.GroupName),
                dal.CreateParameter("@GroupType", (short) (userGroup.GroupType)),
                dal.CreateParameter("@BuildUser", userGroup.BuildUser),
                dal.CreateParameter("@EditUser", userGroup.EditUser)
                );
            if (i == 0)return false;
            var dt = dal.Select("SELECT Id ,BuildDate,EditDate FROM tUserGroup WHERE GroupCode =@GroupCode", out i,
                dal.CreateParameter("@GroupCode", userGroup.GroupCode));
            if (i == 0) return false;
            userGroup.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            userGroup.BuildDate = Convert.ToDateTime(dt.Rows[0]["BuildDate"]);
            userGroup.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]);
            return true;
        }

        /// <summary>
        /// 修改用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CUserGroup userGroup)
        {
            int i;
            dal.Execute("UPDATE tUserGroup SET GroupName=@GroupName,EditUser=@EditUser,EditDate=GETDATE() WHERE Id=@Id", out i,
                dal.CreateParameter("@GroupName",userGroup.GroupName),
                dal.CreateParameter("@EditUser",userGroup.EditUser),
                dal.CreateParameter("@Id",userGroup.Id));
            return i == 1;
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        public static bool Delete(IDal dal, int id ,out CUserGroup hisUserGroup)
        {
            int i;
            hisUserGroup = Get(dal, id);
            if (hisUserGroup == null) return false;
            dal.Execute("DELETE FROM tUserGroup WHERE Id=@Id",out i,
                dal.CreateParameter("@Id",id));
            return i == 1;
        }
    }
}