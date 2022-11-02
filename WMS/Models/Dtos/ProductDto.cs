namespace WMS.Models.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Category { get; set; }
        public string ClientName { get; set; }   
        public string Status { get; set; }
        public DateTime StatusRegistrationDate { get; set; }
    }
}
