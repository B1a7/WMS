using IronBarCode;
using System.Drawing;
using WMS.Models.Dtos.Product;

namespace WMS.Helpers
{
    public static class QRHelper
    {
        private static readonly string filePath;

        static QRHelper()
        {
            var rootPath = Directory.GetCurrentDirectory();
            filePath = $"{rootPath}/ProductDocumentation/QRCode.png";
        }
        public static void GenerateQR(this string str)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var fileName = "QRCode.png";
            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";

            GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(str, 250);
            barcode.SetMargins(10);
            barcode.AddAnnotationTextBelowBarcode(str);
            barcode.ChangeBarCodeColor(Color.Black);
            barcode.SaveAsPng(filePath);
        }
    }
}
