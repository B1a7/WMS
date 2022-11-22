using AutoMapper;
using IronPdf;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WMS.Enums;
using WMS.Exceptions;
using WMS.ExtensionMethods;
using WMS.Models;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IPdfGenerator
    {
        (byte[], string, string) GetDocumentation (EntityBase entity, DocumentTypesEnum docEnum);
    }

    public class PdfGenerator : IPdfGenerator
    {
        private readonly string rootPath;
        private readonly IQRHelper _qRHelper;

        public PdfGenerator(IQRHelper qRHelper)
        {
            rootPath = Directory.GetCurrentDirectory();
            _qRHelper = qRHelper;
        }

        private (byte[], string, string) GeneratePdf(string fileName, string filePath, string content)
        {
            var htmlLines = new ChromePdfRenderer();

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

            var pdf = htmlLines.RenderHtmlAsPdf(content)
                .SaveAs(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            return (fileContent, contentType, fileName);
        }


        public (byte[], string, string) GetDocumentation(EntityBase entity, DocumentTypesEnum docEnum)
        {
            var fileName = $"{entity.GetFileName(docEnum)}.pdf";
            var filePath = $"{rootPath}/Documentation/{fileName}";

            //Generate QR code
            string dataQrString = entity.ToQrString();
            _qRHelper.GenerateQR(dataQrString);

            //Generate HTML to convert it to the PDF
            var content = entity.GenerateHTML(docEnum);

            var result = GeneratePdf(fileName, filePath, content);

            return result;
        }
    }
}
