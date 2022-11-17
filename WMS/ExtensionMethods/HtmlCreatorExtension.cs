using System.Text.RegularExpressions;
using WMS.Enums;
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




        private static string GenerateLabel(this EntityBase entity, string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string content;

                content = reader.ReadToEnd();
                content = content.Replace("~IdNumber~", entity.Id.ToString());
                content = content.Replace("~Name~", entity.Name.ToString());

                return content;
            }
        }

        private static string GenerateSupplierDocument(this EntityBase entity, string filePath)
        {

            using (var reader = new StreamReader(filePath))
            {
                string content;

                content = reader.ReadToEnd();
                content = content.Replace("~IdNumber~", entity.Id.ToString());
                content = content.Replace("~Name~", entity.Name.ToString());
                content = content.Replace("~Email~", ((Supplier)entity).Email.ToString());
                content = content.Replace("~PhoneNumber~", ((Supplier)entity).PhoneNumber.ToString());

                return content;
            }
        }
      
        private static string GenerateProductDocument(this EntityBase entity, string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string content;

                content = reader.ReadToEnd();
                content = content.Replace("~IdNumber~", entity.Id.ToString());
                content = content.Replace("~Name~", entity.Name.ToString());
                content = content.Replace("~Quantity~", ((Product)entity).Quantity.ToString());
                content = content.Replace("~IsAvaiable~", ((Product)entity).IsAvaiable.ToString());
                content = content.Replace("~Size~", ((Product)entity).Size.ToString());
                content = content.Replace("~ProductionDate~", ((Product)entity).ProductionDate.ToShortDateString());

                return content;
            }
        }


        public static string GenerateHTML(this EntityBase entity, DocumentTypesEnum docEnum)
        {
            var htmlContent = string.Empty;

            var documentName = entity.GetFileName(docEnum);

            var filePath = $"{rootPath}/Documentation/{documentName}.html";

            if (docEnum.ToString().Equals("document"))
            {
                if (entity.GetType().Name.Equals("Product"))
                    htmlContent =  entity.GenerateProductDocument(filePath);

                if (entity.GetType().Name.Equals("Supplier"))
                    htmlContent =  entity.GenerateSupplierDocument(filePath);
            }
            else if (docEnum.ToString().Equals("label"))
                htmlContent =  entity.GenerateLabel(filePath);

            return htmlContent;
        }

    }
}
