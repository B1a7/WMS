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
        (byte[], string, string) GenerateSupplierLabel(int id);
        (byte[], string, string) GenerateSupplierDocument(int id);
    }

    public class DocumentationService : IDocumentationService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<DocumentationService> _logger;
        private readonly IMapper _mapper;
        private readonly IPdfHepler _pdfHepler;

        public DocumentationService(WMSDbContext dbContext, ILogger<DocumentationService> logger, IMapper mapper,
            IPdfHepler pdfHepler)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _pdfHepler = pdfHepler;
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

            var result = _pdfHepler.GetDocumentation(id,"ProductLabel");   

            return result;
        }

        public (byte[], string, string) GenerateSupplierLabel(int id)
        {
            var supplier = _dbContext.Suppliers
                .FirstOrDefault(p => p.Id == id);

            if (supplier == null)
                throw new NotFoundException("Cannot find product");

            var supplierQR = _mapper.Map<ProductQRDto>(supplier)
                .ProductQrToString();

            supplierQR.GenerateQR();

            var result = _pdfHepler.GetDocumentation(id, "SupplierLabel");

            return result;
        }
        
        public (byte[], string, string) GenerateProductDocument(int id)
        {
            var supplier = _dbContext.Suppliers
                .FirstOrDefault(p => p.Id == id);

            if (supplier == null)
                throw new NotFoundException("Cannot find product");

            var supplierQR = _mapper.Map<ProductQRDto>(supplier)
                .ProductQrToString();

            supplierQR.GenerateQR();

            var result = _pdfHepler.GetDocumentation(id, "ProductDocument");

            return result;
        }

        public (byte[], string, string) GenerateSupplierDocument(int id)
        {
            var supplier = _dbContext.Suppliers
                .FirstOrDefault(p => p.Id == id);

            if (supplier == null)
                throw new NotFoundException("Cannot find product");

            var supplierQR = _mapper.Map<ProductQRDto>(supplier)
                .ProductQrToString();

            supplierQR.GenerateQR();

            var result = _pdfHepler.GetDocumentation(id, "SupplierDocument");

            return result;
        }
         
    }
}