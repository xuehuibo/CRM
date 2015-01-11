using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class UserBll
    {
        public static CUser[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT a.*,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode ORDER BY UserCode", out i);
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUser
                {
                    Id = Convert.ToInt16(row["Id"]),
                    UserCode = Convert.ToString(row["UserCode"]).Trim(),
                    UserName = Convert.ToString(row["UserName"]).Trim(),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString("yyyy年M月d日H时m分s秒"),
                    BuildUser = Convert.ToString(row["BuildUser"]).Trim(),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString("yyyy年M月d日H时m分s秒"),
                    EditUser = Convert.ToString(row["EditUser"]).Trim(),
                    DeptCode = Convert.IsDBNull(row["DeptCode"]) ? null : Convert.ToString(row["DeptCode"]).Trim(),
                    DeptName = Convert.IsDBNull(row["DeptName"]) ? null : Convert.ToString(row["DeptName"]).Trim(),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    GroupCode = Convert.IsDBNull(row["GroupCode"]) ? null : Convert.ToString(row["GroupCode"]).Trim(),
                    GroupName = Convert.IsDBNull(row["GroupName"]) ? null : Convert.ToString(row["GroupName"]).Trim()
                }).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public static int CountPeople(IDal dal, string groupCode)
        {
            int i;
            var c = dal.Select("SELECT COUNT(1) AS People FROM tUser WHERE GroupCode=@GroupCode", out i,
                dal.CreateParameter("@GroupCode", groupCode));
            return Convert.IsDBNull(c.Rows[0]["People"]) ? 0 : Convert.ToInt16(c.Rows[0]["People"]);
        }

        public static CUser Get(IDal dal, int id)
        {
            int i;
            var dt =
                dal.Select(
                    "SELECT a.*,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode where a.Id=@Id",
                    out i,
                    dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUser
                {
                    Id = Convert.ToInt16(row["Id"]),
                    UserCode = Convert.ToString(row["UserCode"]),
                    UserName = Convert.ToString(row["UserName"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString("yyyy年M月d日H时m分s秒"),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString("yyyy年M月d日H时m分s秒"),
                    EditUser = Convert.ToString(row["EditUser"]),
                    DeptCode = Convert.IsDBNull(row["DeptCode"]) ? null : Convert.ToString(row["DeptCode"]).Trim(),
                    DeptName = Convert.IsDBNull(row["DeptName"]) ? null :Convert.ToString(row["DeptName"]).Trim(),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    GroupCode = Convert.IsDBNull(row["GroupCode"]) ? null : Convert.ToString(row["GroupCode"]).Trim(),
                    GroupName = Convert.IsDBNull(row["GroupName"]) ? null : Convert.ToString(row["GroupName"]).Trim()
                }).First();
        }

        public static bool Create(IDal dal, CUser user)
        {
            int i;
            var pwd = MD5.Create().ComputeHash(Encoding.Default.GetBytes(user.UserCode+ user.Md5));
            var deptCode = dal.CreateParameter("@DeptCode", DbType.String);
            if (string.IsNullOrEmpty(user.DeptCode))
            {
                deptCode.Value = DBNull.Value;
            }
            else
            {
                deptCode.Value = user.DeptCode;
            }
            var groupCode = dal.CreateParameter("@GroupCode", DbType.String);
            if (string.IsNullOrEmpty(user.GroupCode))
            {
                groupCode.Value = DBNull.Value;
            }
            else
            {
                groupCode.Value = user.GroupCode;
            }
            dal.Execute("INSERT INTO tUser( UserCode ,UserName ,UPassword ,DeptCode ,GroupCode ,Enabled  ,BuildUser ,EditUser) VALUES  ( @UserCode ,@UserName,@UPassword, @DeptCode , @GroupCode, @Enabled , @BuildUser,@EditUser)", out i,
                dal.CreateParameter("@UserCode",user.UserCode),
                dal.CreateParameter("@UserName",user.UserName),
                dal.CreateParameter("@UPassword",pwd),
                deptCode,
                groupCode,
                dal.CreateParameter("@Enabled",user.Enabled),
                dal.CreateParameter("@BuildUser",user.BuildUser),
                dal.CreateParameter("@EditUser",user.EditUser));
            if (i == 0) return false;
            var dt =
                dal.Select(
                    "SELECT a.Id,b.DeptName,c.GroupName,a.BuildUser,a.BuildDate,a.EditUser,a.EditDate,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode where a.UserCode=@UserCode",
                    out i,
                    dal.CreateParameter("@UserCode", user.UserCode));
            if (i == 0) return false;
            user.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            user.BuildDate = Convert.ToDateTime(dt.Rows[0]["BuildDate"]).ToString("yyyy年M月d日H时m分s秒");
            user.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]).ToString("yyyy年M月d日H时m分s秒");
            user.DeptName = Convert.IsDBNull(dt.Rows[0]["DeptName"])
                ? null
                : Convert.ToString(dt.Rows[0]["DeptName"]).Trim();
            user.GroupName = Convert.IsDBNull(dt.Rows[0]["GroupName"])
                ? null
                : Convert.ToString(dt.Rows[0]["GroupName"]).Trim();
            user.BuildUser = Convert.ToString(dt.Rows[0]["BuildUser"]).Trim();
            user.EditUser = Convert.ToString(dt.Rows[0]["EditUser"]).Trim();
            return true;
        }

        public static bool Update(IDal dal, CUser user)
        {
            int i;
            dal.Execute("UPDATE tUser SET UserName=@UserName,DeptCode=@DeptCode,GroupCode=@GroupCode ,EditDate=GETDATE(),EditUser=@EditUser WHERE Id=@Id",out i,
                dal.CreateParameter("@UserName",user.UserName),
                dal.CreateParameter("@DeptCode",user.DeptCode),
                dal.CreateParameter("@GroupCode",user.GroupCode),
                dal.CreateParameter("@EditUser",user.EditUser),
                dal.CreateParameter("@Id",user.Id));
            return i == 1;
        }

        public static bool Delete(IDal dal, int id,out CUser hisUser)
        {
            int i;
            hisUser = Get(dal, id);
            if (hisUser == null) return false;
            dal.Execute("DELETE FROM tUser WHERE Id=@id",out i,
                dal.CreateParameter("@Id",id));
            return i == 1;
        }
    }
}