namespace WMS.Models.Dtos.SupplierDtos
{
    public class SupplierDetailDto
    {
        public int Id { get;}
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
