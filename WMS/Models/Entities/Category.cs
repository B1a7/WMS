namespace WMS.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HSCode { get; set; }

        public virtual List<Product> Product { get; set; }
    }
}
