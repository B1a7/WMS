namespace WMS.Models.Entities
{
    public class Recipient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        //public virtual List<Product> Products { get; set; }
    }
}
