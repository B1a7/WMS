namespace WMS.Models.Entities
{
    public class ProductStatus
    {
        public int Id { get; set; }
        public DateTime DateStatus { get; set; }
        public bool IsActive { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int StatusId { get; set; }
        public virtual Status Status { get; set; }
    }
}
