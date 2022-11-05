namespace WMS.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int IsAvaiable { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Size { get; set; }
        public string Position { get; set; }

        public virtual List<ProductCategory> ProductCategories { get; set; }

        public virtual List<ProductStatus> ProductStatuses { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
}
