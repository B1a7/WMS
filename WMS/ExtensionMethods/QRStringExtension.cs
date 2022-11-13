using WMS.Models.Dtos.Documentation;

namespace WMS.ExtensionMethods
{
    public static class QRStringExtension
    {
        public static string ProductQrToString(this ProductQrDto dto)
        {
            var id = dto.ProductId != null ? dto.ProductId.ToString() : string.Empty;
            var name = dto.Name != null ? dto.Name.ToString() : string.Empty;

            var result = $"Product:{id}:{name}";

            return result;
        }

        public static string SupplierQrToString(this SupplierQrDto dto)
        {
            var id = dto.SupplierId != null ? dto.SupplierId.ToString() : string.Empty;
            var name = dto.Name != null ? dto.Name.ToString() : string.Empty;

            var result = $"Supplier:{id}:{name}";

            return result;
        }
    }
}
