namespace WMS.Models.Entities
{
    public class Journal
    {
        public int Id { get; set; }
        public DateTime OperationDate { get; set; }
        public string OperationType { get; set; }
        public int UserId { get; set; }
    }
}
