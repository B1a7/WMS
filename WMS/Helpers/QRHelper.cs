using AutoMapper;
using IronBarCode;
using System.Drawing;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Dtos.DocumentationDtos;
using WMS.Models.Dtos.ProductDtos;

namespace WMS.Helpers
{
    public interface IQRHelper
    {
        void GenerateQR(string content);
        ScanQrBase ScanQR(string fullPath);
    }

    public class QRHelper : IQRHelper
    {
        private readonly string rootPath;
        private readonly WMSDbContext _dbContext;
        private readonly IMapper _mapper;

        public QRHelper(WMSDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            rootPath = Directory.GetCurrentDirectory();
        }

        public void GenerateQR(string content)
        {
            var fileName = "QRCode.png";
            var filePath = $"{rootPath}/Documentation/{fileName}";

            GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(content, 250);
            barcode.SetMargins(10);
            barcode.AddAnnotationTextBelowBarcode(content);
            barcode.ChangeBarCodeColor(Color.Black);
            barcode.SaveAsPng(filePath);
        }

        public ScanQrBase ScanQR(string fullPath)
        {
            var encodedQR = BarcodeReader.Read(fullPath);
            if (encodedQR == null)
                throw new IncorrectInputException("Cannot read QR code ");

            string[] subs = encodedQR.First().Value.Split(':');
            var type = subs.First();
            var isIdCorrect = Int32.TryParse(subs[1], out int typeId);

            if (!isIdCorrect)
                throw new IncorrectInputException("QrCode is not in our database");

            if (type == QRTypesEnum.Product.ToString())
            {
                var product = _dbContext.Products
                    .FirstOrDefault(p => p.Id == typeId);

                var result = _mapper.Map<ProductScanQrDto>(product);

                return result;
            }

            if (type == QRTypesEnum.Supplier.ToString())
            {
                var supplier = _dbContext.Suppliers
                    .FirstOrDefault(p => p.Id == typeId);

                var result = _mapper.Map<SupplierScanQrDto>(supplier);

                return result;
            }

            return null;

        }
    }
}
