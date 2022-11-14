using System.Text.RegularExpressions;
using WMS.Models.Entities;

namespace WMS.ExtensionMethods
{
    public static class HtmlCreatorExtension
    {
        private static readonly string rootPath;

        static HtmlCreatorExtension()
        {
            rootPath = Directory.GetCurrentDirectory();
        }


        public static void CreateLabel(this Product product)
        {
            var filePath = $"{rootPath}/Documentation/Label.html";
            var objReader = new StreamReader(filePath);
            var content = objReader.ReadToEnd();
            objReader.Close();

            content = Regex.Replace(content, "id", product.Id.ToString());
            content = Regex.Replace(content, "name", product.Name.ToString());

            var writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        public static void CreateLabel(this Supplier supplier)
        {
            var filePath = $"{rootPath}/Documentation/Label.html";
            var objReader = new StreamReader(filePath);
            var content = objReader.ReadToEnd();
            objReader.Close();

            content = Regex.Replace(content, "id", supplier.Id.ToString());
            content = Regex.Replace(content, "name", supplier.Name.ToString());

            var writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        public static void CreateDocument()
        {
        }

    }
}
