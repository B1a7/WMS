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
        (byte[], string, string) GenerateDocumentation<T>(int id, string docType) where T : EntityBase;

        ScanQrBase ScanQrCode(IFormFile file);
    }

    public class DocumentationService : IDocumentationService
    {
        private readonly WMSDbContext _dbContext;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IQRHelper _qRHelper;


        public DocumentationService(WMSDbContext dbContext,
            IPdfGenerator pdfGenerator, IQRHelper qRHelper)
        {
            _dbContext = dbContext;
            _pdfGenerator = pdfGenerator;
            _qRHelper = qRHelper;
        }


        public (byte[], string, string) GenerateDocumentation<T>(int id, string docType) where T : EntityBase
        {
            if (!Enum<DocumentTypesEnum>.IsDefined(docType.ToLower()))
                throw new BadRequestException("Wrong document Type");

            var docEnum = (DocumentTypesEnum)Enum.Parse(typeof(DocumentTypesEnum), docType.ToLower());

            var data = _dbContext.Set<T>()                
                .FirstOrDefault(p => p.Id == id);

            if (data == null)
                throw new NotFoundException("Cannot find product");

            var result = _pdfGenerator.GetDocumentation(data, docEnum);

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