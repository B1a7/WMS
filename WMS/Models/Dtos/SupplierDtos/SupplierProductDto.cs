namespace WMS.Models.Dtos.SupplierDtos
{
    public class SupplierProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public string? Position { get; set; }
        public bool IsAvaiable { get; set; }

        public DateTime DateStatus { get; set; }
        public string PackageStatus { get; set; }
    }
}
