using WMS.Models.Entities;

namespace WMS.Models.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int IsAvaiable { get; set; }

        public virtual List<Status> Statuses { get; set; }

    }
}
