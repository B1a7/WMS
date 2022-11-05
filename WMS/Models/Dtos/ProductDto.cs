namespace WMS.Models.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Position { get; set; }

        public string SupplierName { get; set; }   
        public DateTime DateStatus { get; set; }
        public string PackageStatus { get; set; }

    }
}
