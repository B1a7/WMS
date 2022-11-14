using AutoMapper;
using WMS.Enums;
using WMS.Exceptions;
using WMS.ExtensionMethods;
using WMS.Helpers;
using WMS.Models;
using WMS.Models.Dtos.Documentation;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface IDocumentationService
    {
        (byte[], string, string) GenerateProductLabel(int id);

        (byte[], string, string) GenerateProductDocument(int id);

        (byte[], string, string) GenerateSupplierLabel(int id);

        (byte[], string, string) GenerateSupplierDocument(int id);
         
        ScanQrBase ScanQrCode(IFormFile file);
    }

    public class DocumentationService : IDocumentationService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<DocumentationService> _logger;
        private readonly IMapper _mapper;
        private readonly IPdfHepler _pdfHepler;
        private readonly IQRHelper _qRHelper;

        public DocumentationService(WMSDbContext dbContext, ILogger<DocumentationService> logger, IMapper mapper,
            IPdfHepler pdfHepler, IQRHelper qRHelper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _pdfHepler = pdfHepler;
            _qRHelper = qRHelper;
        }


        private (byte[], string, string) GenerateProductDoc(int id, string docType)
        {
            var product = _dbContext.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Cannot find product");

            var productQR = _mapper.Map<ProductQrDto>(product).ProductQrToString();
            _qRHelper.GenerateQR(productQR);

            product.CreateLabel();

            var result = _pdfHepler.GetDocumentation(docType);

            return result;
        }

        private (byte[], string, string) GenerateSupplierDoc(int id, string docType)
        {
            var supplier = _dbContext.Suppliers
                .FirstOrDefault(p => p.Id == id);

            if (supplier == null)
                throw new NotFoundException("Cannot find supplier");

            var supplierQR = _mapper.Map<SupplierQrDto>(supplier)
                .SupplierQrToString();

            _qRHelper.GenerateQR(supplierQR);

            var result = _pdfHepler.GetDocumentation(docType);

            return result;
        }

        public (byte[], string, string) GenerateProductLabel(int id)
        {
            var result = GenerateProductDoc(id, DocumentationEnum.Label.ToString());
            return result;
        }

        public (byte[], string, string) GenerateSupplierLabel(int id)
        {
            var result = GenerateSupplierDoc(id, DocumentationEnum.Label.ToString());

            return result;
        }

        public (byte[], string, string) GenerateProductDocument(int id)
        {
            var result = GenerateProductDoc(id, DocumentationEnum.ProductDocument.ToString());

            return result;
        }

        public (byte[], string, string) GenerateSupplierDocument(int id)
        {
            var result = GenerateSupplierDoc(id, DocumentationEnum.SupplierDocument.ToString());

            return result;
        }

        public ScanQrBase ScanQrCode(IFormFile file)
        {
            if (file == null)
                throw new NotFoundException("File not found");

            if (ImageFileContentType.ContentType.Contains(file.ContentType))
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fileName = file.Name;
                var fullPath = $"{rootPath}/Documentation/ScannedImage.png";

                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                
                var result  = _qRHelper.ScanQR(fullPath);

                return result;
                
            }    
            else
                throw new BadImageFormatException();

            
        }
    }
}