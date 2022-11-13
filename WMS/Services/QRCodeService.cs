using WMS.Models;
using System.Drawing;
using AutoMapper;
using WMS.Models.Dtos.Product;
using WMS.Helpers;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WMS.Exceptions;
using ZXing;

namespace WMS.Services
{
    public interface IQRCodeService
    {
        string GenerateProductQRCode(int id);
    }

    public class QRCodeService : IQRCodeService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<QRCodeService> _logger;
        private readonly IMapper _mapper;

        public QRCodeService(WMSDbContext dbContext, ILogger<QRCodeService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public string GenerateProductQRCode(int id)
        {
            var product = _dbContext.Products
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Cannot find product");

            var productQR = _mapper.Map<ProductQRDto>(product)
                .ProductQRToString();

            var barcodeWriter = new BarcodeWriter<string>();

            barcodeWriter.Format = BarcodeFormat.QR_CODE;

            var qrCode = barcodeWriter.Write(productQR);
                         
            return productQR;
        }




        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
        //    QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    Bitmap qrCodeImage = qrCode.GetGraphic(20);
        //return View(BitmapToBytes(qrCodeImage));
    }
}
