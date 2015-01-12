using System;
using System.Configuration;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class CustContactBll
    {
        /// <summary>
        /// 每页显示20条数据
        /// </summary>
        private const int PageLength = 20;

        /// <summary>
        /// 按照条件查询指定联系人
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="customerCode"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static CCustContact[] List(IDal dal, string customerCode,int page)
        {
            int i;
            var dt =
                dal.Select(
                    "SELECT * FROM tCustContact Where CustomerCode=@CustomerCode",
                    (page-1)*PageLength+1, PageLength, out i,
                    dal.CreateParameter("@CustomerCode", customerCode)
                    );
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CCustContact
                {
                    Id = Convert.ToInt16(row["Id"]),
                    CustomerCode = Convert.ToString(row["CustomerCode"]),
                    ContactCode = Convert.ToString(row["ContactCode"]),
                    ContactName = Convert.ToString(row["ContactName"]),
                    Phone1 = Convert.ToString(row["Phone1"]),
                    Phone2 = Convert.ToString(row["Phone2"]),
                    Email = Convert.ToString(row["Email"]),
                    Remark = Convert.ToString(row["Remark"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    Enabled = Convert.ToBoolean(row["Enabled"])
                }).ToArray();
        }

        public static CCustContact Get(IDal dal,int id)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tCustContact WHERE Id=@Id", out i,
                dal.CreateParameter("@Id",id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CCustContact
                {
                    Id = Convert.ToInt16(row["Id"]),
                    ContactCode = Convert.ToString(row["ContactCode"]),
                    ContactName = Convert.ToString(row["ContactName"]),
                    CustomerCode = Convert.ToString(row["CustomerCode"]),
                    Phone1 = Convert.ToString(row["Phone1"]),
                    Phone2 = Convert.ToString(row["Phone2"]),
                    Email = Convert.ToString(row["Email"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    Remark = Convert.ToString(row["Remark"]),
                    Enabled = Convert.ToBoolean(row["Enabled"])
                }).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="custContact"></param>
        /// <param name="editUser"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CCustContact custContact,string editUser)
        {
            int i;
            dal.Execute(
                "INSERT INTO tCustContact( CustomerCode ,ContactCode,ContactName ,Phone1 ,Phone2 ,Email ,Remark ,BuildDate ,BuildUser ,EditDate ,EditUser,Enabled )VALUES  ( @CustomerCode ,@SerialNo,@ContactName ,@Phone1 ,@Phone2,@Email ,@Remark ,GETDATE(),@BuildUser ,GETDATE(),@EditUser,@Enabled)",
                out i,
                dal.CreateParameter("@CustomerCode", custContact.CustomerCode.Trim()),
                dal.CreateParameter("@ContactCode", custContact.ContactCode.Trim()),
                dal.CreateParameter("@ContactName", custContact.ContactName.Trim()),
                dal.CreateParameter("@Phone1", custContact.Phone1.Trim()),
                dal.CreateParameter("@Phone2", custContact.Phone2.Trim()),
                dal.CreateParameter("@Email", custContact.Email.Trim()),
                dal.CreateParameter("@Remark", custContact.Remark.Trim()),
                dal.CreateParameter("@BuildUser", editUser),
                dal.CreateParameter("@EditUser", editUser),
                dal.CreateParameter("@Enabled",custContact.Enabled));
            if (i == 0) return false;
            var dt = dal.Select("SELECT Id,BuildUser,BuildDate,EditUser,EditDate FROM tCustContact WHERE ContactCode=@ContactCode",
                out i,
                dal.CreateParameter("@ContactCode", custContact.ContactCode));
            if (i == 0) return false;
            custContact.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            custContact.BuildUser = Convert.ToString(dt.Rows[0]["BuildUser"]);
            custContact.EditUser = Convert.ToString(dt.Rows[0]["EditUser"]);
            custContact.BuildDate = Convert.ToDateTime(dt.Rows[0]["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            custContact.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="custContact"></param>
        /// <param name="editUser"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CCustContact custContact,string editUser)
        {
            int i;
            dal.Execute("UPDATE tCustContact SET ContactName=@ContactName,CustomerCode=@CustomerCode,Phone1=@Phone,Phone2=@Phone,Email=@Email,Remark=@Remark,EditUser=@EditUser,EditDate=GETDATE(),Enabled=@Enabled Where Id=@Id", out i,
                dal.CreateParameter("@ContactName",custContact.ContactName.Trim()),
                dal.CreateParameter("@CustomerCode",custContact.CustomerCode.Trim()),
                dal.CreateParameter("@Phone1",custContact.Phone1.Trim()),
                dal.CreateParameter("@Phone2",custContact.Phone2.Trim()),
                dal.CreateParameter("@Email",custContact.Email.Trim()),
                dal.CreateParameter("@Remark",custContact.Remark.Trim()),
                dal.CreateParameter("@EditUser", editUser),
                dal.CreateParameter("@Enabled", custContact.Enabled),
                dal.CreateParameter("@Id",custContact.Id));
            if (i == 0) return false;
            var dt = dal.Select("Select EditUser,EditDate From tCustContact Where Id=@Id", out i,
                dal.CreateParameter("@Id", custContact.Id));
            if (i == 0) return false;
            custContact.EditUser = Convert.ToString(dt.Rows[0]["EditUser"]);
            custContact.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            return true;
        }
    }
}