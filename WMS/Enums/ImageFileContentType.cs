namespace WMS.Enums
{
    public static class ImageFileContentType
    {
        public static List<string> ContentType { get; }
        public static string Jpg { get { return "image/jpg"; } }
        public static string Png { get { return "image/png"; } }

        static ImageFileContentType()
        {
            ContentType = new List<string>();
            ContentType.Add(Jpg);
            ContentType.Add(Png);
        }
    }
}
