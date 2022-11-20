using WMS.Models.Entities;

namespace WMS.Models.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public string CategoryName { get; set; }
        public string HSCode { get; set; }
    }
}
