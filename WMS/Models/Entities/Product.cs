namespace WMS.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsAvaiable { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Size { get; set; }
        public string Position { get; set; }


        public virtual List<Category> Categories { get; set; }

        public virtual List<Status> Statuses { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
}
