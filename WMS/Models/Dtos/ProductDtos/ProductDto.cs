namespace WMS.Models.Dtos.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public bool IsAvaiable { get; set; }

        public string SupplierName { get; set; }
        public DateTime DateStatus { get; set; }
        public string PackageStatus { get; set; }

        public List<string> CategoryName { get; set; }

    }
}
