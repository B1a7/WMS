namespace WMS.Models.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
 