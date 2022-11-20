namespace WMS.Models.Entities
{
    public class Journal
    {
        public int Id { get; set; }
        public DateTime OperationDate { get; set; }
        public string OperationType { get; set; }
        public string OperationTarget { get; set; }
        public int TargetId { get; set; }
        public string UserId { get; set; }
    }
}
