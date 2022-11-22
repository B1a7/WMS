using WMS.Models.Entities;

namespace WMS.Models.Dtos.ProductDtos
{
    public class ProductStatusDto
    {
        public int Id { get; }
        public string PackageStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateStatus { get; set; }
    }
}
