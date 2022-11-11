namespace WMS.Models.Entities
{
    public class Layout
    {
        public int Id { get; set; }
        public string PositionXYZ { get; set; }
        public string SpotSize { get; set; }

        public virtual Product Product { get; set; }
    }
}
