namespace CRM.Models
{
    public class CCheckResultModel
    {
        public UniqueCheckStatus Status { get; set; }

        public string ErrorText { get; set; }
    }

    public enum UniqueCheckStatus
    {
        错误 = 0,
        正确 = 1,
        警告 = 2
    }
}