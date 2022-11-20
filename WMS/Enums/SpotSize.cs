namespace WMS.Enums
{
    public static class SpotSize
    {
        public static List<string> SpotSizes { get; }
        public static string Small { get { return "small"; } }
        public static string Medium { get { return "medium"; } }
        public static string Large { get { return "large"; } }

        static SpotSize()
        {
            SpotSizes = new List<string>();
            SpotSizes.Add(Small);
            SpotSizes.Add(Medium);
            SpotSizes.Add(Large);
        }
    }
}
