namespace WMS.Models.Dtos.DocumentationDtos
{
    public class SupplierScanQrDto : ScanQrBase
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
