using System;

namespace CRM.Models
{
    public class CDept
    {
        public int? Id { get; set; }

        public string DeptCode { get; set; }

        public string DeptName { get; set; }

        public string ParentCode { get; set; }

        public DateTime BuildDate { get; set; }

        public string BuildUser { get; set; }

        public DateTime EditDate { get; set; }

        public string EditUser { get; set; }

        public CDept[] Childs { get; set; }
    }
}