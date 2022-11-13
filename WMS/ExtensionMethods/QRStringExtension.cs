using WMS.Models.Dtos.Customer;
using WMS.Models.Dtos.Product;

namespace WMS.ExtensionMethods
{
    public static class QRStringExtension
    {
        public static string ProductQrToString(this ProductQRDto dto)
        {
            var id = dto.ProductId != null ? dto.ProductId.ToString() : string.Empty;
            var name = dto.Name != null ? dto.Name.ToString() : string.Empty;

            var result = $"{id}:{name}";

            return result;
        }

        public static string SupplierQrToString(this SupplierQRDto dto)
        {
            var id = dto.SupplierId != null ? dto.SupplierId.ToString() : string.Empty;
            var name = dto.Name != null ? dto.Name.ToString() : string.Empty;

            var result = $"{id}:{name}";

            return result;
        }
    }
}
