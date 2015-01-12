namespace CRM.Models
{
    public class CCustContact
    {
        public int Id { get; set; }

        public string ContactCode { get; set; }

        public string ContactName { get; set; }

        public string CustomerCode { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Email { get; set; }

        public string Remark { get; set; }

        public string BuildDate { get; set; }

        public string BuildUser { get; set; }

        public string EditDate { get; set; }

        public string EditUser { get; set; }

        public bool Enabled { get; set; }

    }
}