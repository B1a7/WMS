namespace WMS.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //kod hs cpv
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
