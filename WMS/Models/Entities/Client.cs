namespace WMS.Models.Entities
{
    public class Client
    {

        //dziedziczenie lub drugatabela 
        public int Id { get; set; }
        public string Name { get; set; }  
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //public bool IsRecipient { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public int ProductId { get; set; }
        //lista produktów
        public virtual Product Product { get; set; }

    }
}
 