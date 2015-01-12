namespace CRM.Models
{
    public class CCustomer
    {
        public int Id { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        public CustomerStatus Status { get; set; }

        public string Remark { get; set; }

        public string BuildDate { get; set; }

        public string BuildUser { get; set; }

        public string EditDate { get; set; }

        public string EditUser { get; set; }

        public string Owner { get; set; }

        public bool Enabled { get; set; }
    }

    public enum CustomerStatus
    {
        已淘汰=-1,
        初建=0,
        较少联系=1,
        联系频繁=2,
    }
}