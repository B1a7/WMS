namespace WMS.Enums
{
    public static class PackageStatus
    {
        public static List<string> PackageStatuses { get; }
        public static string OutOfWarehouse { get { return "out of warehouse"; } }
        public static string Delivered { get { return "delivered"; } }
        public static string Sent { get { return "sent"; } }
        public static string PlacedInWarehouse { get { return "placed in warehouse"; } }

        static PackageStatus()
        {
            PackageStatuses = new List<string>();
            PackageStatuses.Add(OutOfWarehouse);
            PackageStatuses.Add(Sent);
            PackageStatuses.Add(PlacedInWarehouse);
            PackageStatuses.Add(Delivered);
        }
    }
}
