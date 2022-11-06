using WMS.Models.Entities;

namespace WMS.Models.Dtos.Product
{
    public class ProductStatusDto
    {
        public string PackageStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateStatus { get; set; }
    }
}
