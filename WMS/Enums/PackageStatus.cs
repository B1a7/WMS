namespace WMS.Enums
{
    public static class PackageStatus
    {
        public static List<string> PackageStatuses { get; }
        public static string OutOfWarehouse { get { return "Out Of Warehouse"; } }
        public static string Delivered { get { return "Delivered"; } }
        public static string Sent { get { return "Sent"; } }
        public static string PlacedInWarehouse { get { return "Placed In Warehouse"; } }

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
