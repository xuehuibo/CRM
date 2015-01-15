using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class CustomerBll
    {
        /// <summary>
        /// 页长
        /// </summary>
        private const int PageLength = 20;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="customerCode"></param>
        /// <param name="customerName"></param>
        /// <param name="owner"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static CCustomer[] List(IDal dal,string customerCode,string customerName,string owner,int page)
        {
            var ps = new List<IDbDataParameter>();
            var sql = new StringBuilder(100);
            sql.Append(" SELECT * FROM tCustomer WHERE 1=1 ");
            if (!string.IsNullOrEmpty(customerCode))
            {
                sql.Append(" AND CustomerCode=@CustomerCode ");
                ps.Add(dal.CreateParameter("@CustomerCode",customerCode));
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                sql.Append(" AND CustomerName=@CustomerName ");
                ps.Add(dal.CreateParameter("@CustomerName",customerName));
            }

            if (!string.IsNullOrEmpty(owner))
            {
                sql.Append(" AND Owner=@Owner ");
                ps.Add(dal.CreateParameter("@Owner", owner));
            }
            else
            {
                sql.Append(" AND Owner IS NULL ");
            }
            int i;
            var dt = dal.Select(sql.ToString(), (page - 1)*PageLength + 1, PageLength, out i, ps.ToArray());
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CCustomer()
                {
                    Id = Convert.ToInt16(row["Id"]),
                    CustomerCode = Convert.ToString(row["CustomerCode"]),
                    CustomerName = Convert.ToString(row["CustomerName"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    Owner = Convert.ToString(row["Owner"]),
                    Remark = Convert.ToString(row["Remark"]),
                    Status = (CustomerStatus)Convert.ToInt16(row["Status"])
                }).ToArray();
        }

        /// <summary>
        /// 检测用户编码唯一性
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public static CCheckResultModel CheckCustomerCodeUnique(IDal dal, string customerCode)
        {
            int i;
            dal.Select("SELECT Id FROM tCustomer WHERE CustomerCode=@CustomerCode ", out i,
                dal.CreateParameter("@CustomerCode", customerCode));
            return i == 0 ? new CCheckResultModel { Status = UniqueCheckStatus.正确 }
                : new CCheckResultModel { Status = UniqueCheckStatus.错误, ErrorText = "客户编码重复" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CCustomer Get(IDal dal, int id)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tCustomer WHERE Id=@Id", out i,
                dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CCustomer
                {
                    Id = Convert.ToInt16(row["Id"]),
                    CustomerCode = Convert.ToString(row["CustomerCode"]),
                    CustomerName = Convert.ToString(row["CustomerName"]),
                    Status = (CustomerStatus)Convert.ToInt16(row["Status"]),
                    Remark = Convert.ToString(row["Remark"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    Owner = Convert.ToString(row["Owner"])
                }).First();
        }

        public static bool Create(IDal dal, CCustomer customer,string editUser)
        {
            int i;
            var owner = string.IsNullOrEmpty(customer.Owner) ? 
                dal.CreateParameter("@Owner", DBNull.Value) 
                : dal.CreateParameter("@Owner", customer.Owner.Trim());
            dal.Execute(
                "INSERT INTO tCustomer ( CustomerCode ,CustomerName ,Status ,Remark ,BuildDate ,BuildUser ,EditDate ,EditUser ,Owner,Enabled) VALUES  ( @CustomerCode ,@CustomerName ,@Status ,@Remark ,GETDATE(),@BuildUser ,GETDATE(),@EditUser ,@Owner,@Enabled)",
                out i,
                dal.CreateParameter("@CustomerCode", customer.CustomerCode.Trim()),
                dal.CreateParameter("@CustomerName", customer.CustomerName.Trim()),
                dal.CreateParameter("@Status", (short) customer.Status),
                dal.CreateParameter("@Remark", customer.Remark.Trim()),
                dal.CreateParameter("@BuildUser", editUser),
                dal.CreateParameter("@EditUser", editUser),
                owner,
                dal.CreateParameter("@Enabled", customer.Enabled)
                );
            if (i == 0) return false;
            var dt = dal.Select("SELECT Id,BuildDate,BuildUser,EditDate,EditUser FROM tCustomer WHERE CustomerCode=@CustomerCode", out i,
                dal.CreateParameter("@CustomerCode",customer.CustomerCode));
            if (i == 0) return false;
            customer.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            customer.BuildDate =
                Convert.ToDateTime(dt.Rows[0]["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            customer.BuildUser = Convert.ToString(dt.Rows[0]["BuildUser"]);
            customer.EditDate =
                Convert.ToDateTime(dt.Rows[0]["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            customer.EditUser = Convert.ToString(dt.Rows[0]["EditUser"]);
            return true;
        }

        public static bool Update(IDal dal, CCustomer customer,string editUser)
        {
            int i;
            dal.Execute(
                "UPDATE tCustomer SET CustomerName=@CustomerName,Status=@Status,Remark=@Remark,EditDate=GETDATE(),EditUser=@EditUser,Owner=@Owner,Enabled=@Enabled WHERE Id=@Id",
                out i,
                dal.CreateParameter("@CustomerName", customer.CustomerName.Trim()),
                dal.CreateParameter("@Status", (short) customer.Status),
                dal.CreateParameter("@Remark", customer.Remark.Trim()),
                dal.CreateParameter("@EditUser", editUser),
                dal.CreateParameter("@Owner", customer.Owner.Trim()),
                dal.CreateParameter("@Enabled", customer.Enabled),
                dal.CreateParameter("@Id",customer.Id)
                );
            return i == 1;
        }
    }
}