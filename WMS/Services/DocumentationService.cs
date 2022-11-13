using AutoMapper;
using IronPdf;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WMS.Exceptions;
using WMS.ExtensionMethods;
using WMS.Helpers;
using WMS.Models;
using WMS.Models.Dtos.Product;

namespace WMS.Services
{
    public interface IDocumentationService
    {
        (byte[], string, string) GenerateProductLabel(int id);
        (byte[], string, string) GenerateProductDocument(int id);
        string GenerateSupplierLabel(int id);
        (byte[], string, string) GenerateSupplierDocument(int id);
    }

    public class DocumentationService : IDocumentationService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<DocumentationService> _logger;
        private readonly IMapper _mapper;


        public DocumentationService(WMSDbContext dbContext, ILogger<DocumentationService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }


        public (byte[], string, string) GenerateProductLabel(int id)
        {
            var product = _dbContext.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Cannot find product");

            var productQR = _mapper.Map<ProductQRDto>(product)
                .ProductQrToString();

            productQR.GenerateQR();

            var rootPath = Directory.GetCurrentDirectory();
            var fileName = "QRCode.png";
            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";


            var fileExist = File.Exists(filePath);
            if (!fileExist)
                throw new NotFoundException("file not exists");

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

            var fileContent = File.ReadAllBytes(filePath);

            return (fileContent, contentType, fileName);
        }
        
        public string GenerateSupplierLabel(int id)
        {
            return string.Empty;
        }
        
        public (byte[], string, string) GenerateProductDocument(int id)
        {
            var htmlLines = new ChromePdfRenderer();

            var product = _dbContext.Products
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Cannot find product");

            var productLabel = _mapper.Map<ProductQRDto>(product)
                .ProductQrToString();

            var rootPath = Directory.GetCurrentDirectory();
            var fileName = "productLabel.pdf";
            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";

            var fileExist = File.Exists(filePath);
            if (!fileExist)
                throw new NotFoundException("file not exists");

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

            var pdf = htmlLines.RenderHtmlAsPdf($"<h1> + {productLabel} </h1>")
                .SaveAs(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            return (fileContent, contentType, fileName);
        }

        public (byte[], string, string) GenerateSupplierDocument(int id)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var fileName = "rdest.jpg";

            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";

            var fileExist = File.Exists(filePath);
            if (!fileExist)
                throw new NotFoundException("file not exists");

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

            var fileContent = File.ReadAllBytes(filePath);

            return (fileContent, contentType, fileName);
        }

    }
}