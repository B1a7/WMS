namespace WMS.Models.Dtos.ProductDtos
{
    public class ProductDetailDto
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Size { get; set; }
        public string? Position { get; set; }
        public bool IsAvaiable { get; set; }

        public DateTime DateStatus { get; set; }
        public string PackageStatus { get; set; }

        public string SupplierName { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPhoneNumber { get; set; }

        public List<string> CategoryName { get; set; }
        public List<string> CategoryHSCode { get; set; }
    }
}
