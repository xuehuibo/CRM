using System;
using System.Data;
using System.Linq;
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
                    UserCode = Convert.ToString(row["UserCode"]),
                    UserName = Convert.ToString(row["UserName"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"])
                }).ToArray();
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
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"])
                }).First();
        }

        public static bool Create(IDal dal, CUser user)
        {
            int i;
            dal.Execute("INSERT INTO tUser( UserCode ,UserName  ,DeptCode ,GroupCode ,Enabled  ,BuildUser ,EditUser) VALUES  ( @UserCode ,@UserName, @DeptCode , @GroupCode, @Enabled , @BuildUser,@EditUser)",out i,
                dal.CreateParameter("@UserCode",user.UserCode),
                dal.CreateParameter("@UserName",user.UserName),
                dal.CreateParameter("@DeptCode",user.DeptCode),
                dal.CreateParameter("@GroupCode",user.GroupCode),
                dal.CreateParameter("@Enabled",user.Enabled),
                dal.CreateParameter("@BuildUser",user.BuildUser),
                dal.CreateParameter("@EditUser",user.EditUser));
            if (i == 0) return false;
            var dt =
                dal.Select(
                    "SELECT a.Id,a.BuildDate,a.EditDate,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode where a.UserCode=@UserCode",
                    out i,
                    dal.CreateParameter("@UserCode", user.UserCode));
            if (i == 0) return false;
            user.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            user.BuildDate = Convert.ToDateTime(dt.Rows[0]["BuildDate"]);
            user.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]);
            user.DeptName = Convert.ToString(dt.Rows[0]["DeptName"]);
            user.GroupName = Convert.ToString(dt.Rows[0]["GroupName"]);
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