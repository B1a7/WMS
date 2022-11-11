namespace WMS.Enums
{
    public static class SpotSize
    {
        public static List<string> SpotSizes { get; }
        public static string Small { get { return "Small"; } }
        public static string Medium { get { return "Medium"; } }
        public static string Large { get { return "Large"; } }

        static SpotSize()
        {
            SpotSizes = new List<string>();
            SpotSizes.Add(Small);
            SpotSizes.Add(Medium);
            SpotSizes.Add(Large);
        }
    }
}
