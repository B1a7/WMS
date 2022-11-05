namespace WMS.Models.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string PackageStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateStatus { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
