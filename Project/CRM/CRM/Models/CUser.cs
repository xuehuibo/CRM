using System;

namespace CRM.Models
{
    public class CUser
    {
        public int? Id { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string DeptCode { get; set; }

        public string DeptName { get; set; }

        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public bool Enabled { get; set; }

        public DateTime BuildDate { get; set; }

        public string BuildUser { get; set; }

        public DateTime EditDate { get; set; }

        public string EditUser { get; set; }
    }
}