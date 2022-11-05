namespace WMS.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        // waga 
        // wymiar 
        // data ważności 

        public virtual List<Category> Category { get; set; }
        public virtual List<Status> Status { get; set; }
        public virtual Client Client { get; set; }


    }
}
