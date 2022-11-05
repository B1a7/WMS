namespace WMS.Models.Entities
{
    public class Client
    {

        public int Id { get; set; }
        public string Name { get; set; }  
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Product> Product { get; set; }

    }
}
 