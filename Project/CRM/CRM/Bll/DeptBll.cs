using System;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class DeptBll
    {
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public static CDept[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tDept ORDER BY DeptCode", out i);
            if (i == 0)
            {
                return null;
            }
            return (from DataRow row in dt.Rows
                where Convert.ToString(row["DeptCode"]).Trim() == "root"
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode = Convert.IsDBNull(row["DeptCode"])?string.Empty:Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.IsDBNull(row["DeptName"])?string.Empty:Convert.ToString(row["DeptName"]),
                    ParentCode = Convert.IsDBNull(row["ParentCode"]) ? string.Empty : Convert.ToString(row["ParentCode"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.IsDBNull(row["BuildUser"]) ? string.Empty : Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.IsDBNull(row["EditUser"]) ? string.Empty : Convert.ToString(row["EditUser"]),
                    Childs = GetChilds(Convert.ToString(row["DeptCode"]).Trim(), dt)
                }).ToArray();
        }

        /// <summary>
        /// 获取指定节点的子节点
        /// </summary>
        /// <returns></returns>
        private static CDept[] GetChilds(string parentCode,DataTable dt)
        {
            if (parentCode == null) return null;
            return (from DataRow row in dt.Rows
                where Convert.ToString(row["ParentCode"]).Trim() == parentCode
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode = Convert.IsDBNull(row["DeptCode"]) ? string.Empty : Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.IsDBNull(row["DeptName"]) ? string.Empty : Convert.ToString(row["DeptName"]),
                    ParentCode = Convert.IsDBNull(row["ParentCode"]) ? string.Empty : Convert.ToString(row["ParentCode"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.IsDBNull(row["BuildUser"]) ? string.Empty : Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.IsDBNull(row["EditUser"]) ? string.Empty : Convert.ToString(row["EditUser"]),
                    Childs = GetChilds(Convert.ToString(row["DeptCode"]).Trim(), dt)
                }
                ).ToArray();
        }

        /// <summary>
        /// 读取指定Dept
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CDept Get(IDal dal, int id)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tDept WHERE Id=@Id", out i,
                dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    ParentCode = Convert.ToString(row["ParentCode"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]),
                    EditUser = Convert.ToString(row["EditUser"])
                }).First();
        }

        /// <summary>
        /// 新建部门
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CDept dept)
        {
            int i;
            dal.Execute("INSERT INTO tDept( DeptCode ,DeptName ,ParentCode ,BuildUser EditUser) VALUES  ( @DeptCode ,@DeptName ,@ParentCode ,@BuildUser ,@EditUser )",out i,
                dal.CreateParameter("@DeptCode",dept.DeptCode),
                dal.CreateParameter("@DeptName",dept.DeptName),
                dal.CreateParameter("@ParentCode",dept.ParentCode),
                dal.CreateParameter("@BuildUser",dept.BuildUser),
                dal.CreateParameter("@EditUser",dept.EditUser));
            if (i == 0) return false;
            var dt = dal.Select("SELECT Id,BuildDate,EditDate FROM dbo.tDept WHERE DeptCode=@DeptCode",out i,
                dal.CreateParameter("@DeptCode",dept.DeptCode));
            if (i == 0) return false;
            dept.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            dept.BuildDate = Convert.ToDateTime(dt.Rows[0]["BuildDate"]);
            dept.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]);
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CDept dept)
        {
            int i;
            dal.Execute("UPDATE tDept SET DeptName=@DeptName ,EditDate=GETDATE(),EditUser=@EditUser WHERE Id=@Id", out i,
                dal.CreateParameter("@DeptName",dept.DeptName),
                dal.CreateParameter("@EditUser",dept.EditUser),
                dal.CreateParameter("@Id",dept.Id));
            return i == 1;
        }

        public static bool Delete(IDal dal, int id,out CDept hisDept)
        {
            int i;
            hisDept = Get(dal, id);
            if (hisDept == null) return false;
            dal.Execute("DELETE FROM tDept WHERE Id=@Id",out i,
                dal.CreateParameter("@Id",id));
            return i == 1;
        }
    }
}