namespace WMS.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public int Weight { get; set; }
        public string Size { get; set; }


        public virtual List<ProductCategory> ProductCategories { get; set; }

        public virtual List<ProductStatus> ProductStatuses { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        //public int RecipientId { get; set; }
        //public virtual Recipient Recipient { get; set; }

    }
}
