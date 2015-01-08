using System;

namespace CRM.Models
{
    public class CUserGroup
    {
        public int? Id { get; set; }

        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public GroupType GroupType { get; set; }

        public DateTime BuildDate { get; set; }

        public string BuildUser { get; set; }

        public DateTime EditDate { get; set; }

        public string EditUser { get; set; }
    }

    public enum GroupType
    {
        维护者=0,
        使用者=1
    }
}