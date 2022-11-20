using WMS.Enums;
using WMS.Models.Entities;

namespace WMS.ExtensionMethods
{
    public static class StringExtension
    {

        public static string ToQrString(this EntityBase entity)
        {
            var id = entity.Id != null ? entity.Id.ToString() : string.Empty;
            var name = entity.Name != null ? entity.Name.ToString() : string.Empty;

            var result = $"Supplier:{id}:{name}";

            return result;
        }

        public static string GetFileName(this EntityBase entity, DocumentTypesEnum docEnum)
        {
            var documentName = $"{entity.GetType().Name}" +
                $"{docEnum.ToString().First().ToString().ToUpper() + docEnum.ToString().Substring(1)}";

            return documentName;
        }
    }
}
