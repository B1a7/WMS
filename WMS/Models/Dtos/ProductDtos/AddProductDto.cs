using System.ComponentModel.DataAnnotations;
using WMS.Enums;
using WMS.Models.Entities;

namespace WMS.Models.Dtos.ProductDtos
{
    public class AddProductDto
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Size { get; set; }
        public string CategoryName { get; set; }
        public string HSCode { get; set; }
        public bool IsAvaiable { get; } = false;
        public int SupplierId { get; set; }
        public bool IsActive { get; } = true;
        public string Status { get; } = PackageStatus.OutOfWarehouse;
        public DateTime StatusRegistrationDate { get; } = DateTime.Now;

    }
}
