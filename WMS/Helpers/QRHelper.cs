using WMS.Models.Dtos.Product;

namespace WMS.Helpers
{
    public static class QRHelper
    {

        public static string ProductQRToString(this ProductQRDto dto)
        {
            var name = dto.Name != null ? dto.Name.ToString() : string.Empty;
            var isAvaiable = dto.IsAvaiable != null ? dto.IsAvaiable.ToString() : "false" ;
            var quantity = dto.Quantity != null ? dto.Quantity.ToString() : "0";
            var supplier = dto.SupplierName != null ? dto.SupplierName.ToString() : string.Empty;

            var result = $"Name: {name}, " +
                $"Is Avaiable: {isAvaiable}, " +
                $"Quantity: {quantity}, " +
                $" Supplier: {supplier}";

            return result;
        }
    }
}
